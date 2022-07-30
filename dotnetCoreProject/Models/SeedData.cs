using dotnetCoreProject.Controllers.infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Models
{
    public class SeedData
    {

        public static void initialData(IServiceProvider serviceProvider)
        {
            
            using(var context = new shoppingDbContext(serviceProvider.GetRequiredService<DbContextOptions<shoppingDbContext>>()))
            {
                if (context.pages.Any())
                {
                    return;
                }
                else
                {

                    context.pages.AddRange(
                        new Page
                        {
                            title = "Home",
                            slug = "home",
                            content = "home page",
                            sorting = 0
                        },
                        new Page
                        {
                            title = "about us",
                            slug = "about-us",
                            content = "about us page",
                            sorting = 100
                        },
                        new Page
                        {
                            title = "contact",
                            slug = "contact",
                            content = "contact page",
                            sorting = 100
                        },
                        new Page
                        {
                            title = "services",
                            slug = "services",
                            content = "services page",
                            sorting = 100
                        }

                    );

                    context.SaveChanges();

                }
            }
        }

        private IDisposable shoppingDbContext()
        {
            throw new NotImplementedException();
        }
    }
}
