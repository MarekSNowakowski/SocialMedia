using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Domain
{
    public class Votes
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public List<UserData> Upvoters { get; set; }
    }
}
