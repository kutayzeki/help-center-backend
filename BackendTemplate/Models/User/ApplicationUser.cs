using Microsoft.AspNetCore.Identity;

namespace FeedbackHub.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<Feedback.Feedback> Feedbacks{ get; set; }
        public virtual List<CompanyUser.CompanyUser> CompanyUsers { get; set; }


    }

}

