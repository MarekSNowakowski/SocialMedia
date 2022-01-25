using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.WebApp.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Nazwa użytkownika jest wymagana!")]
        [Display(Name = "Nazwa Użytkownika")]
        public string UserName { get; set; }

        public string Email { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane!")]
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
