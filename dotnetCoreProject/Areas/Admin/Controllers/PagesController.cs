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
    public class PagesController : Controller
    {

        private readonly shoppingDbContext context;

        public PagesController(shoppingDbContext context)
        {
            this.context = context;
        }


        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in context.pages orderby p.sorting select p;
            List<Page> listPages = await pages.ToListAsync();


            return View(listPages);
        }

        //get home/admin/pages/details/id
        public async Task<IActionResult> Details(int id)
        {
            Page page = await context.pages.FirstOrDefaultAsync(x => x.id == id);
            if (page == null)
                return NotFound();

            return View(page);
        }


        //get /admin/pages/create
        public IActionResult Create() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page)
        {
            
            if (ModelState.IsValid)
            {
                page.slug = page.title.ToLower().Replace(" ", "-");
                page.sorting = 100;

                var slug = await context.pages.FirstOrDefaultAsync(x => x.slug == page.slug);
                if(slug != null)
                {
                    ModelState.AddModelError("", "the title already exists");
                    return View(page);
                }

                context.pages.Add(page);
                await context.SaveChangesAsync();


                TempData["success"] = "page has been added successfully!";
                
                return RedirectToAction("Index");

            }

            return View(page);



        }


        public async Task<IActionResult> EditAsync(int id)
        {
            var page = await context.pages.FindAsync(id);

            if (page == null)
                return NotFound();
            else
                return View(page);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditAsync(Page page)
        {
            if (ModelState.IsValid)
            {
                page.slug = page.id == 1 ? "home" : page.title.ToLower().Replace(" ", "-");
                var sl = await context.pages.Where(x => x.id != page.id).FirstOrDefaultAsync(x => x.slug == page.slug);
                if(sl != null)
                {
                    ModelState.AddModelError("", "page already exists");
                    return View(page);
                }

                context.Update(page);
                await context.SaveChangesAsync();
                TempData["success"] = "the data has been editted!";

                return RedirectToAction("Edit" , new { id = page.id});

            }

            return View(page);
        }


        public async Task<IActionResult> DeleteAsync(int id)
        {

            var page = await context.pages.FindAsync(id);
            if(page == null)
            {
                TempData["error"] = "page doesn't exists";
            }
            else
            {
                context.pages.Remove(page);
                await context.SaveChangesAsync();
                TempData["success"] = "page has been deleted successfully";

            }

            return RedirectToAction("Index");

        }

        [HttpPost]

        public async Task<IActionResult> reorderAsync(int [] id)
        {
            int count = 1;
            foreach(var x in id)
            {
                var page = await context.pages.FindAsync(x);
                page.sorting = count++;
                context.Update(page);
                await context.SaveChangesAsync();
            }

            return Ok();
        }

    }

    



}