using SocialMedia.Core.Domain;

namespace SocialMedia.Infrastructure.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Post Post { get; set; }
        public UserData Author { get; set; }
    }
}
