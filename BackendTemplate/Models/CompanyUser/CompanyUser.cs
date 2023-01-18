using FeedbackHub.Models.User;
using Microsoft.AspNetCore.Identity;

namespace FeedbackHub.Models.CompanyUser
{
    public class CompanyUser
    {
        public Guid CompanyId { get; set; }
        public string UserId { get; set; }
        public virtual Company.Company Company { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
