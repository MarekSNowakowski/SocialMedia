using Microsoft.AspNetCore.Mvc;
using SocialMedia.Infrastructure.Commands;
using SocialMedia.Infrastructure.Services;
using System.Threading.Tasks;

namespace SocialMedia.WebAPI.Controllers
{
    [Route("[Controller]")]
    public class UserDataController : Controller
    {
        private readonly IUserDataService _userDataRepository;

        public UserDataController(IUserDataService userDataRepository)
        {
            _userDataRepository = userDataRepository;
        }

        // post
        [HttpGet]
        public async Task<IActionResult> BrowseAll()
        {
            var z = await _userDataRepository.BrowseAllAsync();
            return Json(z);
        }

        // post/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserData(int id)
        {
            var z = await _userDataRepository.GetUserDataAsync(id);
            return Json(z);
        }

        // post
        [HttpPost]
        public async Task<IActionResult> AddUserData([FromBody] CreateUserData userData)
        {
            if(userData != null)
            {
                await _userDataRepository.AddUserDataAsync(userData);
                return Created("", userData);  //zwróci 201
            }
            else
            {
                return NoContent();
            }
        }

        // put/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUserData(int id, [FromBody] EditUserData userData)
        {
            await _userDataRepository.EditUserDataAsync(id, userData);
            return NoContent();
        }

        // post/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserData(int id)
        {
            await _userDataRepository.DeleteUserDataAsync(id);
            return NoContent();
        }

        [HttpGet("id/{username}")]
        public async Task<int> GetUserId(string username)
        {
            int id = await _userDataRepository.GetUserId(username);
            return id;
        }
    }
}

