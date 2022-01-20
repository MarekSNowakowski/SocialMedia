using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Post Post { get; set; }
        public UserData Author { get; set; }
        public DateTime Time { get; set; }
    }
}
