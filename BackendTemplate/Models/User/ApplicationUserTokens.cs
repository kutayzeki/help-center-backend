using Microsoft.AspNetCore.Identity;

namespace FeedbackHub.Models.User
{
    public class ApplicationUserTokens : IdentityUserToken<string>
    {
        public DateTime ExpireDate { get; set; }
    }
}
