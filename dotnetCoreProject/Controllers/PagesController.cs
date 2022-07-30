using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetCoreProject.Controllers.infrastructure;
using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetCoreProject.Controllers
{
    public class PagesController : Controller
    {

        private readonly shoppingDbContext context;

        public PagesController(shoppingDbContext context)
        {
            this.context = context;
        }


        public async Task<IActionResult> Page(string slug)
        {

            if(slug == null)
            {
                return View(await context.pages.FirstOrDefaultAsync(x => x.slug == "home"));
            }

            Page page = await context.pages.FirstOrDefaultAsync(x => x.slug == slug);
            if(page == null)
            {
                return NotFound();
            }


            return View(page);


        }
    }
}