using System;

namespace SocialMedia.Infrastructure.DTO
{
    class UserDataDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string AvatarPhotoPath { get; set; }
    }
}
