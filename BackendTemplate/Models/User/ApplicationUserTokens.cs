using Microsoft.AspNetCore.Identity;

namespace BackendTemplate.Models.User
{
    public class ApplicationUserTokens : IdentityUserToken<string>
    {
        public DateTime ExpireDate { get; set; }
    }
}
