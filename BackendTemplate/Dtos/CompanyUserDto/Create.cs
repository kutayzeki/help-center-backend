using FeedbackHub.Models.Company;
using FeedbackHub.Models.User;

namespace FeedbackHub.Dtos.CompanyUserDto
{
    public class CompanyUserCreate
    {
        public Guid CompanyId { get; set; }
        public string UserId { get; set; }
    }
}
