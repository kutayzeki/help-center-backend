using BackendTemplate.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BackendTemplate.Models
{
    public class APIDbContext : IdentityDbContext<ApplicationUser>
    {
        public APIDbContext(DbContextOptions<APIDbContext> dbContext) : base(dbContext) { }


        public DbSet<ApplicationUserTokens> ApplicationUserTokens { get; set; }
    }



}
