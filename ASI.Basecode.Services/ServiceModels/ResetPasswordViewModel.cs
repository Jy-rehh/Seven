using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }

}
