using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetCoreProject.Controllers.infrastructure;
using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetCoreProject.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {

        private readonly shoppingDbContext context;

        public ProductController(shoppingDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var products = await context.products.OrderBy(x => x.Id).Include(x=> x.category)
                .Skip((p-1)*pageSize).Take(pageSize).ToListAsync();


            ViewBag.PageNumber = p;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)(context.products.Count()*1.0 / pageSize));
            ViewBag.range = pageSize;


            return View(products);

        }



        // get product/productByCategory

        public async Task<IActionResult> productByCategory(string catgorySlug , int p = 1)
        {

            Category category = await context.categories.Where(x => x.slug == catgorySlug).FirstOrDefaultAsync();

            if (category == null) return RedirectToAction("Index");

            int pageSize = 6;

            var products = await context.products.Where(x => x.CategoryId == category.id)
                .OrderBy(x => x.Id).Include(x=> x.category).Skip((p - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.category = category.name;
            ViewBag.slug = catgorySlug;
            ViewBag.PageNumber = p;
            
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)(context.products.Where(x=> x.CategoryId == category.id).Count()*1.0 / pageSize));
            ViewBag.PageRange = pageSize;


            return View(products);


        }

    }
}