using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.WebApp.Models
{
    public class PostVM
    {
        //Create properties

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,png,gif,jpeg,bmp,svg")]
        public IFormFile Photo { get; set; }

        //Other properties
        public string PhotoPath { get; set; }
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Author { get; set; }
    }
}
