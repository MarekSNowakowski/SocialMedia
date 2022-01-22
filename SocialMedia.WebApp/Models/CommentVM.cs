using System;

namespace SocialMedia.WebApp.Models
{
    public class CommentVM
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public UserDataVM Author { get; set; }
        public DateTime Time { get; set; }
    }
}
