using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Atelier_2.Models
{
    public class Client : IdentityUser
    {
        public int ClientId { get; set; }

        [Required]
        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

          }
}
