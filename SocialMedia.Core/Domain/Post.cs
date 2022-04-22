using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Domain
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string PhotoPath { get; set; }
        public UserData Author { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Votes > Votes { get; set; }
        public List<Reports> Reports { get; set; }
    }
}
