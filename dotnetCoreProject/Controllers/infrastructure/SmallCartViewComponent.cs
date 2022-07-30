using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Controllers.infrastructure
{
    public class SmallCartViewComponent:ViewComponent
    {

        //private readonly shoppingDbContext context;

        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.getJson<List<CartItem>>("cart");
            SmallCartViewModel smallCartView;

            if(cart == null || cart.Count == 0)
            {
                smallCartView = null;
            }
            else
            {
                smallCartView = new SmallCartViewModel
                {
                    totalItems = cart.Sum(x => x.quantity),
                    quntity = cart.Sum(x => x.quantity * x.total)
                };
            }

            return View(smallCartView);

        }



    }
}
