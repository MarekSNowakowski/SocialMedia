using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public byte[] Image { get; set; }
        public string Author { get; set; }
    }
}
