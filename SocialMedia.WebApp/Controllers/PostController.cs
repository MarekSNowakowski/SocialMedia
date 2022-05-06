using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SocialMedia.WebApp.Models;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using IronXL;

namespace SocialMedia.WebApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PostController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //JSON token data
        private const string SECRET = "SuperTajneHaslo2137";
        private const string NAME = "Marek";
        private const string EMAIL = "01153053@pw.edu.pl";
        private const string ADRESS = "http://www.nowakom3.pl";

        public PostController(IConfiguration configuration, ILogger<PostController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public ContentResult GetHostUrl()
        {
            var result = _configuration["RestApiUrl:HostUrl"];
            return Content(result);
        }

        private string CN()
        {
            string cn = ControllerContext.RouteData.Values["controller"].ToString();
            return cn;
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Name", NAME),
                new Claim(JwtRegisteredClaimNames.Email, EMAIL)
            };

            var token = new JwtSecurityToken(
                issuer: ADRESS,
                audience: ADRESS,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials,
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            List<PostVM> postList = new List<PostVM>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        postList = JsonConvert.DeserializeObject<List<PostVM>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Getting posts failed!");
                return View(ex);
            }

            return View(postList);    //view is strongly typed
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            PostVM post = new PostVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        post = JsonConvert.DeserializeObject<PostVM>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Getting posts failed!");
                return View(ex);
            }

            return View(post);    //view is strongly typed
        }

        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Reports()
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            List<PostVM> postList = new List<PostVM>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        postList = JsonConvert.DeserializeObject<List<PostVM>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Getting posts failed!");
                return View(ex);
            }

            postList = postList.FindAll(x => x.Reports.Count > 0);  // Get only reported posts

            return View(postList);    //view is strongly typed
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            PostVM s = new PostVM();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        s = JsonConvert.DeserializeObject<PostVM>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Editing post with ID {id} failed!");
                return View(ex);
            }

            if (!CheckIfCorrectUser(s.Author.Username))
            {
                return View(new Exception("Post editing try without valid user!"));
            }

            return View(s);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostVM s)
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            PostVM result = new PostVM();

            if (s.Photo != null)
            {
                // If a new photo is uploaded, the existing photo must be
                // deleted. So check if there is an existing photo and delete
                if (s.PhotoPath != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                        "images", s.PhotoPath);
                    System.IO.File.Delete(filePath);
                }
                // Save the new photo in wwwroot/images folder and update
                // PhotoPath property of the employee object
                s.PhotoPath = ProcessUploadedFile(s.Photo);
                _logger.LogInformation($"Created photo name:  {s.PhotoPath}");
            }
            else
            {
                _logger.LogWarning("Image was not created during edition of a post!");
            }

            try
            {
                s.Photo = null;

                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.PutAsync($"{_restpath}/{s.Id}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<PostVM>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Editing post with ID {s.Id} failed!");
                return View(ex);
            }

            _logger.LogInformation($"Editing post with ID {s.Id} succeded!");
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var s = JsonConvert.DeserializeObject<PostVM>(apiResponse);

                        if (s.PhotoPath != null)
                        {
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                "images", s.PhotoPath);
                            System.IO.File.Delete(filePath);
                        }

                        if (!CheckIfCorrectUser(s.Author.Username))
                        {
                            throw new Exception("Post editing try without valid user!");
                        }
                    }
                    string _votes_restpath = GetHostUrl().Content + "votes/post/" + id;
                    using (var response = await httpClient.DeleteAsync($"{_votes_restpath}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                    string _reports_restpath = GetHostUrl().Content + "reports/post/" + id;
                    using (var response = await httpClient.DeleteAsync($"{_reports_restpath}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.DeleteAsync($"{_restpath}/{id}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Deleting post with ID {id} failed!");
                return View(ex);
            }

            _logger.LogInformation($"Deleting post with ID: {id} succeded!");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            PostVM s = new PostVM();
            return await Task.FromResult(View(s));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostVM s)
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            int resultID;

            if (s.Photo != null)
            {
                // If a new photo is uploaded, the existing photo must be
                // deleted. So check if there is an existing photo and delete
                if (s.PhotoPath != null)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                        "images", s.PhotoPath);
                    System.IO.File.Delete(filePath);
                }
                // Save the new photo in wwwroot/images folder and update
                // PhotoPath property of the employee object
                s.PhotoPath = ProcessUploadedFile(s.Photo);
                _logger.LogInformation($"Created photo name:  {s.PhotoPath}");
            }
            else
            {
                _logger.LogWarning("Image was not created during creation of a post!");
            }
            try
            {
                s.Photo = null;
                // Set author
                s.AuthorId = await GetUserId();

                if (!User.Identity.IsAuthenticated)
                {
                    return View(new Exception("Post creation try without valid user!"));
                }

                if (s.AuthorId == -1)
                {
                    return View(new Exception("User not found!"));
                }

                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.PostAsync($"{_restpath}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        resultID = JsonConvert.DeserializeObject<int>(apiResponse);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Creating post failed!");
                return View(ex);
            }

            _logger.LogInformation($"Creating post with ID: {resultID} succeded!");
            return RedirectToAction(nameof(Index));
        }
        private string ProcessUploadedFile(IFormFile Photo)
        {
            string uniqueFileName = null;

            if (Photo != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        private bool CheckIfCorrectUser(string author)
        {
            return author == User.Identity.Name || User.IsInRole("admin") || User.IsInRole("Moderator");
        }

        private async Task<int> GetUserId()
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + "userdata/id/" + User.Identity.Name;

            int id = -1;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        id = int.Parse(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Getting user id failed! Username: {User.Identity.Name}, Exception: {ex.Message}");
            }

            return id;    //view is strongly typed
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(IFormCollection formFields)
        {
            string comment = formFields["comment"];
            string id_str = formFields["postid"];
            int id = int.Parse(id_str);

            if (string.IsNullOrEmpty(comment))
            {
                return RedirectToAction(nameof(Index));
            }

            CommentVM result;

            CommentVM commentVM = new CommentVM()
            {
                Content = comment,
                AuthorId = await GetUserId(),
                PostId = id,
                Time = DateTime.Now
            };

            var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + "comment";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(commentVM);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.PostAsync($"{_restpath}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<CommentVM>(apiResponse);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Adding comment failed!");
                return View(ex);
            }

            _logger.LogInformation($"Creating comment: \"{result.Content}\" succeded!");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpvotePost(IFormCollection formFields)
        {
            string id_str = formFields["postid"];
            string from_reports = formFields["fromReports"];

            int id = int.Parse(id_str);

            string alreadyUpvoted_str = formFields["alreadyUpvoted"];
            bool alreadyUpvoted = bool.Parse(alreadyUpvoted_str);

            var tokenString = GenerateJSONWebToken();
            
            string apiResponse;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    int userId = await GetUserId();

                    if (alreadyUpvoted)
                    {
                        string _restpath = GetHostUrl().Content + "votes/post/" + id + "/user/" + userId;
                        string jsonString = System.Text.Json.JsonSerializer.Serialize(await GetUserId());
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        using (var response = await httpClient.DeleteAsync(_restpath))
                        {
                            apiResponse = await response.Content.ReadAsStringAsync();
                        }
                    }
                    else
                    {
                        string _restpath = GetHostUrl().Content + "votes/post/" + id;
                        string jsonString = System.Text.Json.JsonSerializer.Serialize(userId);
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(_restpath, content))
                        {
                            apiResponse = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Upvoting post failed!");
                return View(ex);
            }

            _logger.LogInformation($"Upvoting post: \"{id}\" succeded!");

            if (from_reports != null && from_reports == "yes")
                return RedirectToAction(nameof(Reports));
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ReportPost(IFormCollection formFields)
        {
            string id_str = formFields["postid"];
            int id = int.Parse(id_str);

            string alreadyUpvoted_str = formFields["alreadyReported"];
            bool alreadyUpvoted = bool.Parse(alreadyUpvoted_str);

            var tokenString = GenerateJSONWebToken();
            string apiResponse;

            try
            {
                string _restpath_user = GetHostUrl().Content + "userdata/" + await GetUserId();
                UserDataVM userData = new UserDataVM();


                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    int userId = await GetUserId();

                    if (alreadyUpvoted)
                    {
                        string _restpath = GetHostUrl().Content + "reports/post/" + id + "/user/" + userId;
                        string jsonString = System.Text.Json.JsonSerializer.Serialize(await GetUserId());
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        using (var response = await httpClient.DeleteAsync(_restpath))
                        {
                            apiResponse = await response.Content.ReadAsStringAsync();
                        }
                    }
                    else
                    {
                        string _restpath = GetHostUrl().Content + "reports/post/" + id;
                        string jsonString = System.Text.Json.JsonSerializer.Serialize(userId);
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(_restpath, content))
                        {
                            apiResponse = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Reporting post failed!");
                return View(ex);
            }

            _logger.LogInformation($"Reporting post: \"{id}\" succeded!");
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin,moderator")]
        public async Task<ActionResult> DeleteReports(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string _reports_restpath = GetHostUrl().Content + "reports/post/" + id;
                    using (var response = await httpClient.DeleteAsync($"{_reports_restpath}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Deleting reports failed!");
                return View(ex);
            }

            _logger.LogInformation($"Deleting reports of post with ID: {id} succeded!");
            return RedirectToAction(nameof(Reports));
        }

        [Authorize(Roles = "admin,moderator")]
        public async Task<ActionResult> Statistics()
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            List<PostVM> postList = new List<PostVM>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        postList = JsonConvert.DeserializeObject<List<PostVM>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Getting posts failed!");
                return View(ex);
            }

            return View(postList);    //view is strongly typed
        }

        [Authorize(Roles = "admin,moderator")]
        public async Task<ActionResult> GenerateReport()
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            List<PostVM> postList = new List<PostVM>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        postList = JsonConvert.DeserializeObject<List<PostVM>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Getting posts failed!");
                return View(ex);
            }

            GenerateWorkbook(postList);

            return RedirectToAction("Statistics");
        }

        private void GenerateWorkbook(List<PostVM> postList)
        {
            WorkBook workbook = WorkBook.Create(ExcelFileFormat.XLSX);
            var sheet = workbook.CreateWorkSheet("posts_sheet");

            sheet["A1"].Value = "Id";
            sheet["B1"].Value = "Title";
            sheet["C1"].Value = "Time";
            sheet["D1"].Value = "Upvotes";
            sheet["E1"].Value = "Comments";
            sheet["F1"].Value = "Reports";
            sheet["G1"].Value = "Author Id";
            sheet["H1"].Value = "Author Username";
            sheet["I1"].Value = "Author email";

            sheet.SaveAs($"post_report_{DateTime.Now:yyyy}_{DateTime.Now:MM}_{DateTime.Now:dd}.csv");

        }
    }
}
