﻿using System.ComponentModel.DataAnnotations;
namespace Atelier_2.ViewModels
{
   
        public class RegisterViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]

            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
        
            public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Adresse")]
        public string Address { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Numéro de téléphone")]
        public string PhoneNumber { get; set; }

    }
    }
