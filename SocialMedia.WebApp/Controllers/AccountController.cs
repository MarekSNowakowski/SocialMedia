using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SocialMedia.WebApp.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public ContentResult GetHostUrl()
        {
            var result = _configuration["RestApiUrl:HostUrl"];
            return Content(result);
        }

        private const string CN = "userdata";

        public IActionResult Login()
        {
            return View();
        }

        //Loging POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            // returns user name to login
            var user = await _userManager.FindByNameAsync(loginVM.UserName);

            if(user != null)
            {
                // login try
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Niepoprawna nazwa użytkownika lub hasło...");

            return View(loginVM);
        }

        public IActionResult Register()
        {
            return View(new LoginVM());
        }

        //Registering POST
        [HttpPost]
        public async Task<IActionResult> Register(LoginVM loginVM)
        {
            if(ModelState.IsValid)
            {
                var user = new IdentityUser() { UserName = loginVM.UserName };
                var result = await _userManager.CreateAsync(user, loginVM.Password);

                if(result.Succeeded)
                {
                    AddUserData(loginVM);
                    return RedirectToAction("Index", "Home");   //(metoda, controller)
                }
            }


            ModelState.AddModelError("", "Niepoprawna nazwa użytkownika lub hasło...");

            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async void AddUserData(LoginVM loginVM)
        {
            string _restpath = GetHostUrl().Content + CN;

            UserDataVM userData = new UserDataVM()
            {
                Username = loginVM.UserName,
                Email = loginVM.Email,
                Birthday = loginVM.Birthday,
                RegistrationTime = DateTime.Now
            };

            UserDataVM result = new UserDataVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(userData);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync($"{_restpath}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<UserDataVM>(apiResponse);
                    }
                }

            }
            catch
            {
                _logger.LogWarning($"Creating user data failed!");
            }

            _logger.LogInformation($"Creating user data for user: {result.Username} succeded!");
        }
    }
}
