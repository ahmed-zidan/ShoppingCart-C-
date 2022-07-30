using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Controllers.infrastructure
{
    public class CategoriesViewComponent : ViewComponent
    {

        private readonly shoppingDbContext context;

        public CategoriesViewComponent(shoppingDbContext context)
        {
            this.context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await context.categories.OrderBy(x => x.sorting).ToListAsync();
            
            return View(categories);
        }


    }
}
