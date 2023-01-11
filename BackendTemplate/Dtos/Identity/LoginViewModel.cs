using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ekip.Core.ViewModels.IdentityViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email adresi zorunlu")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunlu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
