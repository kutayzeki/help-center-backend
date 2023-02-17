using HelpCenter.Models.Company;
using HelpCenter.Models.CompanyUser;
using HelpCenter.Models.Product;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HelpCenter.Dtos.CompanyDto
{
    public class Update
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ImgUrl { get; set; }
        public string Theme { get; set; }
    }
}
