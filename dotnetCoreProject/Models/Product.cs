using dotnetCoreProject.Controllers.infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required , MinLength(2 , ErrorMessage ="min length = 2")]
        public string Name { set; get; }
        public string Slug { set; get; }

        [Required , MinLength(4 , ErrorMessage ="min length = 4")]
        public string Description { set; get; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { set; get; }

        [Display(Name ="Category")]
        [Range(1 , int.MaxValue , ErrorMessage ="you must choose category")]
        public int CategoryId { set; get; }

        
        public string Image { set; get; }


        [ForeignKey("CategoryId")]
        public virtual Category category { set; get; }


        [NotMapped]
        [FileExtension]
        public IFormFile imageUpload { set; get; }

    }
}
