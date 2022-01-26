using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.WebApp.Models
{
    public class UserDataVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public DateTime RegistrationTime { get; set; }
    }
}
