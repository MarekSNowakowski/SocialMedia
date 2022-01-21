using SocialMedia.Core.Domain;
using System;
using System.Collections.Generic;

namespace SocialMedia.Infrastructure.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string PhotoPath { get; set; }
        public UserData Author { get; set; }
        public List<CommentDTO> Comments { get; set; }
    }
}
