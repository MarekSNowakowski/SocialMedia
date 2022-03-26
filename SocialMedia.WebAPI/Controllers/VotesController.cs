using Microsoft.AspNetCore.Mvc;
using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.DTO;
using SocialMedia.Infrastructure.Services;
using System.Threading.Tasks;

namespace SocialMedia.WebAPI.Controllers
{
    [Route("[Controller]")]
    public class VotesController : Controller
    {
        private readonly IVotesService _votesRepository;

        public VotesController(IVotesService votesRepository)
        {
            _votesRepository = votesRepository;
        }

        // votes
        [HttpGet]
        public async Task<IActionResult> BrowseAll()
        {
            var z = await _votesRepository.BrowseAllAsync();
            return Json(z);
        }

        // votes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVotes(int id)
        {
            var z = await _votesRepository.GetVotesAsync(id);
            return Json(z);
        }

        // votes/post/{id}
        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetPostsVotes(int id)
        {
            var z = await _votesRepository.GetVotesAsync(id);
            return Json(z);
        }

        // votes/post/{id}
        [HttpPost("post/{id}")]
        public async Task<IActionResult> AddVotes(int id)
        {
            await _votesRepository.AddVotesAsync(id);
            return Created("", id);  //zwróci 201
        }

        // votes/post/{id}
        [HttpPut("post/{id}")]
        public async Task<IActionResult> UpvotePost(int id, [FromBody] UserDataDTO userData)
        {
            await _votesRepository.UpvotePostAsync(id, userData);
            return NoContent();
        }

        // votes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVotes(int id)
        {
            await _votesRepository.DeleteVotesAsync(id);
            return NoContent();
        }

        // votes/post/{id}
        [HttpDelete("post/{id}")]
        public async Task<IActionResult> DeletePostVotes(int id)
        {
            await _votesRepository.DeletePostVotesAsync(id);
            return NoContent();
        }
    }
}

