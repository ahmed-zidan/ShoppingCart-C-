using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetCoreProject.Controllers.infrastructure;
using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnetCoreProject.Controllers
{
    public class CardController : Controller
    {
        private readonly shoppingDbContext context;


        public CardController(shoppingDbContext context)
        {
            this.context = context;
        }

        // get /cart
        public IActionResult Index()
        {

            List<CartItem> carts = HttpContext.Session.getJson<List<CartItem>>("cart") ?? new List<CartItem>();

            CartViewModel cvm = new CartViewModel
            {
                cartItems = carts,
                GrandTotal = carts.Sum(x => x.price * x.quantity)
            };

            return View(cvm);

        }

        // get /add
        public async Task<IActionResult> add(int id)
        {

            

            Product product = await context.products.FindAsync(id);

            var cart = HttpContext.Session.getJson<List<CartItem>>("cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(x => x.productId == id).FirstOrDefault();

            if(cartItem == null)
            {
                cart.Add(new CartItem(product));

            }
            else
            {
                cartItem.quantity++;

            }

            HttpContext.Session.setJson("cart", cart);

            if(HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return RedirectToAction("index");
            }

            return ViewComponent("SmallCart");

        }


        public IActionResult decrease(int id)
        {

            var card = HttpContext.Session.getJson<List<CartItem>>("cart");
            var cartItem = card.Where(x => x.productId == id).FirstOrDefault();
            if(cartItem.quantity > 1)
            {
                --cartItem.quantity;
            }
            else
            {
                card.RemoveAll(x=>x.productId == cartItem.productId);
            }

            HttpContext.Session.setJson("cart", card);
            if (card.Count == 0)
                HttpContext.Session.Remove("cart");

            return RedirectToAction("Index");


        }

        public IActionResult remove(int id)
        {
            var card = HttpContext.Session.getJson<List<CartItem>>("cart");

            card.RemoveAll(x => x.productId == id);
            if(card.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                HttpContext.Session.setJson("cart", card);
            }

            return RedirectToAction("Index");

        }

        public IActionResult clear()
        {
            HttpContext.Session.Remove("cart");
            //return RedirectToAction("Index");

            if(HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());

            return Ok();

        }




    }
}