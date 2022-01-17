using SocialMedia.Core.Domain;

namespace SocialMedia.Infrastructure.Commands
{
    public class CreateComment
    {
        public string Content { get; set; }
        public Post Post { get; set; }
        public UserData Author { get; set; }
    }
}
