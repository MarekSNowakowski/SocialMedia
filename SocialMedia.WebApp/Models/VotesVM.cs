using System.Collections.Generic;

namespace SocialMedia.WebApp.Models
{
    public class VotesVM
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public UserDataVM Upvoter { get; set; }
    }
}
