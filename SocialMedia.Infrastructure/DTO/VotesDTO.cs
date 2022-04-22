namespace SocialMedia.Infrastructure.DTO
{
    public class VotesDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; } 
        public UserDataDTO Upvoter { get; set; }
    }
}
