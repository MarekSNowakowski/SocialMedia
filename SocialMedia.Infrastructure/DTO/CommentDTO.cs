using SocialMedia.Core.Domain;
using System;

namespace SocialMedia.Infrastructure.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public UserData Author { get; set; }
        public DateTime Time { get; set; }
    }
}
