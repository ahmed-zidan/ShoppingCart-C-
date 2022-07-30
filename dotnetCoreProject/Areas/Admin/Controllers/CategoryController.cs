using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetCoreProject.Controllers.infrastructure;
using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetCoreProject.Areas.Admin.Controllers
{

    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly shoppingDbContext context;

        public CategoryController(shoppingDbContext context)
        {
            this.context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await context.categories.OrderBy(x => x.sorting).ToListAsync());
        }

        //get home/admin/category/create

        public IActionResult Create() => View();


        //post home/admin/category/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {

            var slug = category.name.ToLower().Replace(" ", "-");
            category.slug = slug;

            //check slug exists or not
            var cat = await context.categories.FirstOrDefaultAsync(x => x.slug == category.slug);
            if (cat != null)
            {
                ModelState.AddModelError("", "category already exists");
                return View(category);
            }

            category.sorting = 100;
            await context.categories.AddAsync(category);
            await context.SaveChangesAsync();
            TempData["success"] = "category has been added successfully";

            return RedirectToAction("index");


        }



        //Edit /admin/category/Edit/id

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            var category = await context.categories.FirstOrDefaultAsync(x => x.id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, Category category)
        {

            
                category.slug = category.name.ToLower().Replace(" ", "-");
                var slug = context.categories.Where(x => x.id != id).FirstOrDefault(x => x.slug == category.slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "category already exists");
                    return View(category);
                }

                context.Update(category);
                await context.SaveChangesAsync();
                TempData["success"] = "data has been edited";
                return RedirectToAction("Edit", new { id });

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await context.categories.FirstOrDefaultAsync(x => x.id == id);
            if(category == null)
            {
                TempData["error"] = "category doesn't exists";
            }
            else
            {
                context.categories.Remove(category);
                await context.SaveChangesAsync();
                TempData["success"] = "category has been deleted successfully";
            }

            return RedirectToAction("index");

        }


        [HttpPost]
        public async Task<IActionResult> reorder(int [] id)
        {
            int count = 0;
            foreach(var x in id)
            {
                var category = await context.categories.FindAsync(x);
                category.sorting = count++;
                context.Update(category);
                await context.SaveChangesAsync();
            }

            
            return Ok();

        }


    }

}