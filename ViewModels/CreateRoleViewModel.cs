using System.ComponentModel.DataAnnotations;

namespace Atelier_2.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]

        public string RoleName { get; set; }

    }
}
