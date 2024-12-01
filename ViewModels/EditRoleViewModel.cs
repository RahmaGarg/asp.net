using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atelier_2.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            // Initialize the Users list to ensure it is never null.
            Users = new List<string>();
        }

        // Property for Role ID (used to identify which role is being edited)
        public string Id { get; set; }

        // Required validation for RoleName field to ensure a name is entered
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        // List of user names associated with the role
        public List<string> Users { get; set; }
    }
}
