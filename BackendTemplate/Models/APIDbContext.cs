using HelpCenter.Models.CompanyUser;
using HelpCenter.Models.Feedback;
using HelpCenter.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace HelpCenter.Models
{
    public class APIDbContext : IdentityDbContext<ApplicationUser>
    {
        public APIDbContext(DbContextOptions<APIDbContext> dbContext) : base(dbContext) { }


        public DbSet<ApplicationUserTokens> ApplicationUserTokens { get; set; }
        public DbSet<Company.Company> Companies{ get; set; }
        public DbSet<Company.AccountType> AccountTypes{ get; set; }
        public DbSet<Product.Product> Products { get; set; }
        public DbSet<Feedback.Feedback> Feedbacks { get; set; }
        public DbSet<FeedbackUpvote> FeedbackUpvotes { get; set; }
        public DbSet<HelpCenter.HelpCenter> HelpCenters { get; set; }
        public DbSet<Section.Section> Sections { get; set; }
        public DbSet<Item.Item> Items { get; set; }
        public DbSet<CompanyUser.CompanyUser> CompanyUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product.Product>()
                .HasMany(p => p.Feedbacks)
                .WithOne(f => f.Product)
                .HasForeignKey(f => f.ProductId);            
            
            modelBuilder.Entity<Product.Product>()
                .HasMany(p => p.HelpCenters)
                .WithOne(f => f.Product)
                .HasForeignKey(f => f.ProductId);
            modelBuilder.Entity<HelpCenter.HelpCenter>()
                .HasMany(p => p.Sections)
                .WithOne(f => f.HelpCenter)
                .HasForeignKey(f => f.HelpCenterId);            
            modelBuilder.Entity<Section.Section>()
                .HasMany(p => p.Items)
                .WithOne(f => f.Section)
                .HasForeignKey(f => f.SectionId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.Feedbacks)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Company.AccountType>()
                .HasMany(at => at.Companies)
                .WithOne(c => c.AccountType)
                .HasForeignKey(c => c.AccountTypeId);

            modelBuilder.Entity<Company.Company>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId);

            modelBuilder.Entity<Company.Company>()
                .HasMany(c => c.CompanyUsers)
                .WithOne(c => c.Company)
                .HasForeignKey(c => c.CompanyId);            
            
            modelBuilder.Entity<Feedback.Feedback>()
                .HasMany(b=> b.FeedbackUpvotes)
                .WithOne(c => c.Feedback)
                .HasForeignKey(b => b.FeedbackId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<ApplicationUser>()
               .HasMany(b => b.FeedbackUpvotes)
               .WithOne(b => b.User)
               .HasForeignKey(b => b.UserId);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.CompanyUsers)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<FeedbackUpvote>().HasKey(cu => new
            {
                cu.Id
            });
            modelBuilder.Entity<CompanyUser.CompanyUser>().HasKey(cu => new
            {
                cu.UserId,
                cu.CompanyId
            });
        }
    }
}
