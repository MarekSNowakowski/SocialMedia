using Microsoft.AspNetCore.Mvc;
using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.DTO;
using SocialMedia.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.WebAPI.Controllers
{
    [Route("[Controller]")]
    public class PostController : Controller
    {
        private readonly IPostService _postRepository;

        public PostController(IPostService postRepository)
        {
            _postRepository = postRepository;
        }

        // post
        [HttpGet]
        public async Task<IActionResult> BrowseAll()
        {
            var z = await _postRepository.BrowseAllAsync();
            return Json(z);
        }

        // post/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var z = await _postRepository.GetPostAsync(id);
            return Json(z);
        }

        // post
        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] CreatePost post)
        {
            int id = await _postRepository.AddPostAsync(post);
            return Created("", id);  //zwróci 201
        }

        // put/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPost(int id, [FromBody] EditPost post)
        {
            await _postRepository.EditPostAsync(id, post);
            return NoContent();
        }

        // post/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postRepository.DeletePostAsync(id);
            return NoContent();
        }
    }
}

