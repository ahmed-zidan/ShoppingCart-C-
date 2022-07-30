using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnetCoreProject.Areas.Admin.Controllers
{

    //[Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public RolesController(UserManager<AppUser>userManager , RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index() => View(roleManager.Roles);

        //get roels/create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        //post roles/create
        public async Task<IActionResult> Create([Required,MinLength(2 , ErrorMessage ="length is too short")] string name)
        {
            if (ModelState.IsValid)
            {

                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    TempData["success"] = "role has been created successfully";
                    return RedirectToAction("Index");
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();

            }

            ModelState.AddModelError("", "min length is 2");
            return View();
            
        }

        //get admin/edit/5

        public async Task<IActionResult> Edit(string id)
        {

            IdentityRole role = await roleManager.FindByIdAsync(id);

            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();

            foreach(AppUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }


            return View(new RoleEdit
            {
                role = role,
                members = members,
                nonMembers = nonMembers

            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // post admin/edit

        public async Task<IActionResult> Edit(RoleEdit roleEdit)
        {

            foreach(var user in roleEdit.addIds??new string[] { })
            {
                AppUser appUser = await userManager.FindByIdAsync(user);
                await userManager.AddToRoleAsync(appUser, roleEdit.roleName);
            }


            foreach (var user in roleEdit.DeleteIds ?? new string[] { })
            {
                AppUser appUser = await userManager.FindByIdAsync(user);
                await userManager.RemoveFromRoleAsync(appUser, roleEdit.roleName);
            }

            return Redirect(Request.Headers["Referer"].ToString());

        }

    }
}