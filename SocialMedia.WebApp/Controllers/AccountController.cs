using Microsoft.AspNetCore.Authorization;
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

        public async Task<IActionResult> Details(string username)
        {
            string _restpath_getid = GetHostUrl().Content + "userdata/id/" + username;

            int id = -1;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_restpath_getid))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        id = int.Parse(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Getting user id failed! Username: {username}, Exception: {ex.Message}");
                return View(ex);
            }

            if(id == -1)
            {
                _logger.LogWarning($"Getting user id failed! Username: {username}");
                Exception ex = new Exception("User not found");
                return View(ex);
            }

            string _restpath = GetHostUrl().Content + "userdata/" + id;
            UserDataVM userData = new UserDataVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        userData = JsonConvert.DeserializeObject<UserDataVM>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Getting user data failed! Id: {id}, Exception: {ex.Message}");
                return View(ex);
            }

            return View(userData);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            string _restpath = GetHostUrl().Content + CN;

            UserDataVM s = new UserDataVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        s = JsonConvert.DeserializeObject<UserDataVM>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Editing post with ID {id} failed!");
                return View(ex);
            }

            return View(s);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UserDataVM s)
        {
            string _restpath = GetHostUrl().Content + CN;

            UserDataVM result = new UserDataVM();

            if (User.Identity.Name != s.Username && await _userManager.FindByNameAsync(s.Username) != null)
            {
                ModelState.AddModelError("UsernameTaken", "Nazwa użytkownika jest już zajęta!");
                return View(s);
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PutAsync($"{_restpath}/{s.Id}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<UserDataVM>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Editing user with ID {s.Id} failed!");
                return View(ex);
            }

            // Update username
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.UserName = s.Username;

            // If username changed we need to update identity
            await _userManager.UpdateAsync(user);
            if (s.Username != User.Identity.Name)
            {
                await Logout();
                RedirectToAction(nameof(Login));
            }

            _logger.LogInformation($"Editing user with ID {s.Id} succeded!");
            return RedirectToAction(nameof(Details),new { s.Username });
        }

    }
}
