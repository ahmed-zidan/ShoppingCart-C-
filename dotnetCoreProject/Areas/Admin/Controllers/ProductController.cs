using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dotnetCoreProject.Controllers.infrastructure;
using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dotnetCoreProject.Areas.Admin.Controllers
{

    [Authorize(Roles ="Admin")]
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly shoppingDbContext context;

        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(shoppingDbContext context , IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(int p = 1)
        {

            int pageSize = 5;

            var products = context.products.OrderByDescending(x => x.Id).Include(x => x.category)
                .Skip((p - 1) * pageSize).Take(pageSize);


            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.products.Count() / pageSize);
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;


            return View(await products.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.categories.OrderBy(x => x.sorting), "id", "name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Product product)
        {

            product.Slug = product.Name.ToLower().Replace(" ", "-");
            var pr = await context.products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
            if (pr != null)
            {
                ModelState.AddModelError("", "product already exists");
                return View(product);
            }

            string imgName = "noImage.jpg";

            //has file
            if(product.imageUpload != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                imgName = Guid.NewGuid().ToString() + "_" + product.imageUpload.FileName;
                string filePath = Path.Combine(uploadDir, imgName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.imageUpload.CopyToAsync(fs);
                fs.Close();
                
            }

            product.Image = imgName;

            context.products.Add(product);

            await context.SaveChangesAsync();
            TempData["success"] = "product has been added successfully";
            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Details(int id)
        {
            var product = await context.products.Include(x => x.category).FirstOrDefaultAsync(x => x.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await context.products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return NotFound();

            ViewBag.categoryId = new SelectList(context.categories, "id", "name", product.CategoryId);
            return View(product);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id , Product product)
        {

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");
                
                if(product.imageUpload != null)
                {
                    string uplDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    //remove image
                    if (!product.Image.Equals("noImage.png"))
                    {
                        string oldImg = Path.Combine(uplDir, product.Image);
                        if (System.IO.File.Exists(oldImg))
                        {
                            System.IO.File.Delete(oldImg);
                        }

                    }

                    string imgName = Guid.NewGuid().ToString() + "_" + product.imageUpload.FileName;
                    string filePath = Path.Combine(uplDir, imgName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.imageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imgName;

                }

                context.products.Update(product);
                await context.SaveChangesAsync();


                TempData["success"] = "data has been editted successfully";
                return RedirectToAction("Index");

            }
            return View(product);

        }


        public async Task<IActionResult> Delete(int id)
        {
            var product = await context.products.FindAsync(id);
            if(product == null)
            {
                ModelState.AddModelError("", "product doesn't exists");
                
            }
            else
            {
                if (!product.Image.Equals("noImage.png"))
                {
                    string dir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    string filePath = Path.Combine(dir, product.Image);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                context.products.Remove(product);
                await context.SaveChangesAsync();

                TempData["success"] = "data has been deleted successfully";

            }

            return RedirectToAction("Index");


        }



    }
}