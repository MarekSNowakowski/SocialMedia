using Microsoft.AspNetCore.Mvc;
using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.Services;
using System.Threading.Tasks;

namespace SocialMedia.WebAPI.Controllers
{
    [Route("[Controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentRepository;

        public CommentController(ICommentService commentRepository)
        {
            _commentRepository = commentRepository;
        }

        // post
        [HttpGet]
        public async Task<IActionResult> BrowseAll()
        {
            var z = await _commentRepository.BrowseAllAsync();
            return Json(z);
        }

        // post/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var z = await _commentRepository.GetCommentAsync(id);
            return Json(z);
        }

        // post
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateComment comment)
        {
            await _commentRepository.AddCommentAsync(comment);
            return Created("", comment);  //zwróci 201
        }

        // put/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditComment(int id, [FromBody] EditComment comment)
        {
            await _commentRepository.EditCommentAsync(id, comment);
            return NoContent();
        }

        // post/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentRepository.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}

