using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Domain
{
    public class Votes
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public UserData Upvoter { get; set; }
    }
}
