using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Controllers.infrastructure
{
    [HtmlTargetElement("td" , Attributes = "user-role")]
    public class UserTagHelper : TagHelper
    {

        private readonly UserManager<AppUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        public UserTagHelper(UserManager<AppUser>userManager , RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HtmlAttributeName("user-role")]
        public string roleId { set; get; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //search for role
            IdentityRole role = await roleManager.FindByIdAsync(roleId);

            List<String> users = new List<string>();

            if (role != null)
            {
                foreach (var user in userManager.Users)
                {
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        users.Add(user.UserName);
                    }
                }
            }


            output.Content.SetContent(users.Count == 0 ? "no users" : string.Join(", ", users));


        }
        

        





    }
}
