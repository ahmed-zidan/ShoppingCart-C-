using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Models
{
    public class EditUser
    {
        [EmailAddress , Required]
        public string email { set; get; }

        [DataType(DataType.Password)]
        public string password { set; get; }

        public EditUser() { }
        public EditUser(AppUser appUser)
        {
            this.email = appUser.Email;
            this.password = appUser.PasswordHash;
        }


    }
}
