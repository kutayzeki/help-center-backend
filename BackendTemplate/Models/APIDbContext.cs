using BackendTemplate.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BackendTemplate.Models
{
    public class APIDbContext : IdentityDbContext<AppUser>
    {
        public APIDbContext(DbContextOptions<APIDbContext> dbContext) : base(dbContext) { }
    }
}
