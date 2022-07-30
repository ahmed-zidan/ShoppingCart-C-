using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Controllers.infrastructure
{
    public class FileExtensionAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            var file = value as IFormFile;

            //file exists
            if(file != null)
            {
                var ext = Path.GetExtension(file.FileName);
                string[] exts = { "jpg", "png" };
                bool res = exts.Any(x => ext.EndsWith(x));

                if (!res)
                {
                    return new ValidationResult("extension should be jpg or png!");
                }
                
            }

            return ValidationResult.Success;

        }

    }
}
