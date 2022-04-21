using SocialMedia.Core.Domain;
using System.Collections.Generic;

namespace SocialMedia.Infrastructure.DTO
{
    public class ReportsDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; } 
        public List<UserData> Reporters { get; set; }
        public int Count 
        {
            get
            {
                if (Reporters == null)
                    return -1;

                return Reporters.Count; 
            } 
        }
    }
}
