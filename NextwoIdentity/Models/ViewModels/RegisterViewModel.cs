using System.ComponentModel.DataAnnotations;

namespace NextwoIdentity.Models.ViewModels
{
    public class RegisterViewModel

    {

        [EmailAddress]
        [Required(ErrorMessage ="please enter the email")]
        public string? Email { get; set; }

        [Required(ErrorMessage ="please enter the password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage ="enter confirmation")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="passwords are not match")]
        public string? ConfirmPassword { get; set; }
        public string? Phone { get; set; }


    }
}
