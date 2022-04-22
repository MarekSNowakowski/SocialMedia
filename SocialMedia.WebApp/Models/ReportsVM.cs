using System.Collections.Generic;

namespace SocialMedia.WebApp.Models
{
    public class ReportsVM
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public UserDataVM Reporter { get; set; }
    }
}
