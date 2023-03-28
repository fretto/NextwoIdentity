using System.ComponentModel.DataAnnotations;

namespace NextwoIdentity.Models.ViewModels
{
    public class LoginViewModel
    {

        [EmailAddress]
        [Required(ErrorMessage = "please enter the email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "please enter the password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
