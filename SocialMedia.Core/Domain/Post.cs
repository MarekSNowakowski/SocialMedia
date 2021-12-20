using System;

namespace SocialMedia.Core.Domain
{
    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public byte[] Image { get; set; }
    }
}
