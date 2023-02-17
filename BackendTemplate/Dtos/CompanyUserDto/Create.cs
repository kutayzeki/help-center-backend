using HelpCenter.Models.Company;
using HelpCenter.Models.User;

namespace HelpCenter.Dtos.CompanyUserDto
{
    public class CompanyUserCreate
    {
        public Guid CompanyId { get; set; }
        public string UserId { get; set; }
    }
}
