using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppIdentity.Models;

namespace WebAppIdentity.EF
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public MyDbContext(DbContextOptions<MyDbContext> opts) : base(opts) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<IdentityRole>().HasData(
            //    new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() });
            //modelBuilder.Entity<IdentityRole>().HasData(
            //   new IdentityRole { Name = "User", NormalizedName = "User".ToUpper() });

            base.OnModelCreating(modelBuilder);
        }
    }
}
