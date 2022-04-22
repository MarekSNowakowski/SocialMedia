namespace SocialMedia.Infrastructure.DTO
{
    public class ReportsDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; } 
        public UserDataDTO Reporter { get; set; }
    }
}
