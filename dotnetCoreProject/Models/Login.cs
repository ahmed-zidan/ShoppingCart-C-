using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Models
{
    public class Login
    {

        [Required , EmailAddress]
        public string email { set; get; }

        [Required, DataType(DataType.Password)]
        public string password { set; get; }
        public string returnURL { set; get; }


    }
}
