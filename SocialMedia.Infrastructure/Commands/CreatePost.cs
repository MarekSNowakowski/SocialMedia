using SocialMedia.Core.Domain;
using System;

namespace SocialMedia.Infrastructure.Commands
{
    public class CreatePost
    {
        public string Title { get; set; }
        public string PhotoPath { get; set; }
        public int AuthorID { get; set; }
    }
}
