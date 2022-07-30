using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Models
{
    public class User
    {
        [Required , MinLength(2 , ErrorMessage ="should be greater than 2 characters")]
        [Display(Name ="Username")]
        public string userName { get; set; }

        [Required , EmailAddress]
        public string email { get; set; }

        [Required,DataType(DataType.Password)]
        public string password { get; set; }

        public User() { }

        public User(AppUser user)
        {
            userName = user.UserName;
            email = user.Email;
            password = user.PasswordHash;
        }



    }
}
