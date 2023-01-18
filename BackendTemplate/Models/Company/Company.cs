using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FeedbackHub.Models.Company
{
    public class Company
    {
        public Guid CompanyId { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 5 )]
        public string Name { get; set; }
        [DisallowNull]
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImgUrl { get; set; }
        public string? Theme { get; set; }
        public bool IsActive { get; set; }
        public Guid? AccountTypeId { get; set; }
        public virtual AccountType AccountType { get; set; }
        public virtual List<Product.Product> Products { get; set; }
        public virtual List<CompanyUser.CompanyUser> CompanyUsers{ get; set; }

    }
}
