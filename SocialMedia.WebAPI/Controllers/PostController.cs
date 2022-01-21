using Microsoft.AspNetCore.Mvc;
using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.DTO;
using SocialMedia.Infrastructure.Services;
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
            await _postRepository.AddPostAsync(post);
            return Created("", post);  //zwróci 201
        }

        // put/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditPost(int id, [FromBody] CreatePost post)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpvotePost(int id, [FromBody] UserDataDTO userDataDTO)
        {
            await _postRepository.UpVotePostAsync(id, userDataDTO);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> CommentPost(int id, [FromBody] CommentDTO commentDTO)
        {
            await _postRepository.AddCommentToPostAsync(id, commentDTO);
            return NoContent();
        }
    }
}

