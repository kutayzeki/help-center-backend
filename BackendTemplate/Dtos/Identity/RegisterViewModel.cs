using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ekip.Core.ViewModels.IdentityViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Name surname")]
        [StringLength(60)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password you entered does not match.")]
        public string ConfirmPassword { get; set; }

        public string Role { get;set; }
    }
}
