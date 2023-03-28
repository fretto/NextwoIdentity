using System.ComponentModel.DataAnnotations;


namespace NextwoIdentity.Models.ViewModels
{
    public class EditRoleViewModel
    {

        public EditRoleViewModel() {

            Users = new List<string>();  
        }
        public string? RoleId { get; set; }

        [Required(ErrorMessage="please enter name")]
        public string? RoleName { get; set; } 

        public List<string>? Users { get; set; }
    }
}
