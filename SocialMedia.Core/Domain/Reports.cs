﻿using System.Collections.Generic;

namespace SocialMedia.Core.Domain
{
    public class Reports
    {
        public int Id { get; set; }
        public Post Post { get; set; }
        public UserData Reporter { get; set; } 
    }
}
