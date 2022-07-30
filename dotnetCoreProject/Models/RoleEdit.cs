using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Models
{
    public class RoleEdit
    {

        public IdentityRole role { set; get; }
        public IEnumerable<AppUser> members { get; set; }

        public IEnumerable<AppUser> nonMembers { get; set; }
        public string roleName { get; set; }
        public string [] addIds { get; set; }
        public string [] DeleteIds { get; set; }

    }
}
