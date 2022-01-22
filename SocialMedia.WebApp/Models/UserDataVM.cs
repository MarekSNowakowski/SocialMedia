using System;

namespace SocialMedia.WebApp.Models
{
    public class UserDataVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime RegistrationTime { get; set; }
    }
}
