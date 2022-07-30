using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnetCoreProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IPasswordHasher<AppUser> passwordHasher;
        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser>signInManager,
            IPasswordHasher<AppUser> passwordHasher)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordHasher = passwordHasher;
        }

        //get /account/register
        [AllowAnonymous]
        public IActionResult Register() => View();
    
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(User user)
        {

                AppUser appUser = new AppUser()
                {
                    UserName = user.userName,
                    Email = user.email
                };

                IdentityResult result = await userManager.CreateAsync(appUser , user.password);

                if (result.Succeeded)
                {
                    return RedirectToAction("login");
                }
                else
                {

                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }



            return View(user);

        }

        [AllowAnonymous]
        //get account/login
        public IActionResult Login(string returnURL)
        {
            Login login = new Login()
            {
                returnURL = returnURL
            };
            return View(login);
        }
        

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(Login login)
        {

            //check email
            AppUser appUser = await userManager.FindByEmailAsync(login.email);
            if(appUser != null)
            {
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(appUser, login.password, false, false);

                if (signInResult.Succeeded)
                {

                    return Redirect(login.returnURL ?? "/");

                }


            }

            ModelState.AddModelError("", "faild to login");

            return View(login);

        }


        
        // get account/logout
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return Redirect("/");
        }


        public async Task<IActionResult> Edit()
        {
            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);
            EditUser user = new EditUser(appUser);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult>Edit(EditUser user)
        {

            AppUser appUser = await userManager.FindByNameAsync(User.Identity.Name);

            appUser.Email = user.email;
            if(user.password != null)
            {
                appUser.PasswordHash = passwordHasher.HashPassword(appUser, user.password);
            }

            IdentityResult result = await userManager.UpdateAsync(appUser);

            if (result.Succeeded)
                TempData["success"] = "account has been edited successfully";

            return View(user);


        }


    
    }


    


}