using Microsoft.AspNetCore.Mvc;
using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.DTO;
using SocialMedia.Infrastructure.Services;
using System.Threading.Tasks;

namespace SocialMedia.WebAPI.Controllers
{
    [Route("[Controller]")]
    public class ReportsController : Controller
    {
        private readonly IReportsService _reportsRepository;

        public ReportsController(IReportsService reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        // reports
        [HttpGet]
        public async Task<IActionResult> BrowseAll()
        {
            var z = await _reportsRepository.BrowseAllAsync();
            return Json(z);
        }

        // reports/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVotes(int id)
        {
            var z = await _reportsRepository.GetReportsAsync(id);
            return Json(z);
        }

        // reports/post/{id}
        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetPostsVotes(int id)
        {
            var z = await _reportsRepository.GetPostsReportsAsync(id);
            return Json(z);
        }

        // reports/post/{id}
        [HttpPost("post/{id}")]
        public async Task<IActionResult> AddReports(int id)
        {
            await _reportsRepository.AddReportsAsync(id);
            return Created("", id);  //zwróci 201
        }

        // reports/post/{id}
        [HttpPut("post/{id}")]
        public async Task<IActionResult> ReportPost(int id, [FromBody] UserDataDTO userData)
        {
            await _reportsRepository.ReportPostAsync(id, userData);
            return NoContent();
        }

        // reports/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReports(int id)
        {
            await _reportsRepository.DeleteReportsAsync(id);
            return NoContent();
        }

        // reports/post/{id}
        [HttpDelete("post/{id}")]
        public async Task<IActionResult> DeletePostReports(int id)
        {
            await _reportsRepository.DeletePostReportsAsync(id);
            return NoContent();
        }
    }
}

