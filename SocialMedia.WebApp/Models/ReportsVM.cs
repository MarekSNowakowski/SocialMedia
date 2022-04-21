using System.Collections.Generic;

namespace SocialMedia.WebApp.Models
{
    public class ReportsVM
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public List<UserDataVM> Reporters { get; set; }
        public int Count
        {
            get
            {
                return Reporters.Count;
            }
        }
    }
}
