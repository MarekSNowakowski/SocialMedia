using SocialMedia.Core.Domain;
using System;
using System.Collections.Generic;

namespace SocialMedia.Infrastructure.DTO
{
    public class VotesDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; } 
        public List<UserData> Upvoters { get; set; }
        public int Count 
        {
            get
            {
                return Upvoters.Count; 
            } 
        }
    }
}
