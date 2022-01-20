using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Domain
{
    public class UserData
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime RegistrationTime { get; set; }
    }
}
