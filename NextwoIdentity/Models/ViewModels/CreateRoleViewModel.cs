using System.ComponentModel.DataAnnotations;

namespace NextwoIdentity.Models.ViewModels
{
    public class CreateRoleViewModel
    {

        [Required]
        public string? RoleName { get;set; }
    }
}
