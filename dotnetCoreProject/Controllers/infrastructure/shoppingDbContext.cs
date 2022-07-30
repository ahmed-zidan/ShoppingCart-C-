using dotnetCoreProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Controllers.infrastructure
{
    public class shoppingDbContext: IdentityDbContext<AppUser>
    {

        public shoppingDbContext(DbContextOptions<shoppingDbContext>options):
            base(options)
        {
            
        }
        public DbSet<Page> pages { set; get; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }

    }
}
