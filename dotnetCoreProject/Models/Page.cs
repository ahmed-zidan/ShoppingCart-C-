using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Models
{
    public class Page
    {

        public int id { get; set; }
        [Required , MinLength(2 , ErrorMessage ="minimum length is 2")]
        public string title { get; set; }
        [Required , MinLength(4 , ErrorMessage ="minimum length is 4")]
        public string content { get; set; }
        public string slug { get; set; }
        public int sorting { get; set; }


    }
}
