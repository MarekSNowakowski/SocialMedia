using System;

namespace SocialMedia.Core.Domain
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string PhotoPath { get; set; }
        public string Author { get; set; }
    }
}
