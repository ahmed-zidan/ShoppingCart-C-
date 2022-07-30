using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Models
{
    public class Category
    {


        public int id { get; set; }
        [Required , MinLength(2,ErrorMessage ="minimum length 2")]
        [RegularExpression(@"^[a-zA-Z-]+$" , ErrorMessage ="only letters are allowed")]
        public string name { get; set; }

        [Required]
        public string slug { get; set; }
        public int sorting { get; set; }


    }
}
