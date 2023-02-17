using Microsoft.AspNetCore.Identity;

namespace HelpCenter.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<Feedback.Feedback> Feedbacks{ get; set; }
        public virtual List<Feedback.FeedbackUpvote> FeedbackUpvotes{ get; set; }
        public virtual List<CompanyUser.CompanyUser> CompanyUsers { get; set; }


    }

}

