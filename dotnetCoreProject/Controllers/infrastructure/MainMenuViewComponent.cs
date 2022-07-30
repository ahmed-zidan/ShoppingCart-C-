using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Controllers.infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {

        private readonly shoppingDbContext context;

        public MainMenuViewComponent(shoppingDbContext context)
        {
            this.context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await context.pages.OrderBy(x => x.sorting).ToListAsync();

            return View(pages);
        }


    }
}
