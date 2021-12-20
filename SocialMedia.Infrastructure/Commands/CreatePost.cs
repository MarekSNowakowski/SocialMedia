namespace SocialMedia.Infrastructure.Commands
{
    public class CreatePost
    {
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public string Author { get; set; }
    }
}
