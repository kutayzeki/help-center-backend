﻿using Microsoft.AspNetCore.Identity;

namespace HelpCenter.Models.User
{
    public class ApplicationUserTokens : IdentityUserToken<string>
    {
        public DateTime ExpireDate { get; set; }
    }
}
