using SocialMedia.Core.Domain;
using System;

namespace SocialMedia.Infrastructure.Commands
{
    public class CreateComment
    {
        public string Content { get; set; }
        public int PostID { get; set; }
        public int AuthorID { get; set; }
    }
}
