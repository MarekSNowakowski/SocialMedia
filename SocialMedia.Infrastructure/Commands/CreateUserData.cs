using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Commands
{
    public class CreateUserData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime RegistrationTime { get; set; }
    }
}
