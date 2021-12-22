using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.WebApp.Models
{
    public class PostVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public byte[] Image { get; set; }
        public string Author { get; set; }
    }
}
