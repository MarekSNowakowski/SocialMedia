﻿using Microsoft.AspNetCore.Mvc;
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

            List<PostVM> skiJumpersList = new List<PostVM>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.GetAsync(_restpath))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        skiJumpersList = JsonConvert.DeserializeObject<List<PostVM>>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Getting posts failed!");
                return View(ex);
            }

            return View(skiJumpersList);    //view is strongly typed
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

            if(!CheckIfCorrectUser(s.Author))
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
                // Set author
                s.Author = User.Identity.Name;

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

                        if (!CheckIfCorrectUser(s.Author))
                        {
                            throw new Exception("Post editing try without valid user!");
                        }
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
                _logger.LogWarning("Image was not created during creation of a post!");
            }
            try
            {
                s.Photo = null;
                // Set author
                s.Author = User.Identity.Name;

                if (s.Author.IsNullOrEmpty())
                {
                    return View(new Exception("Post creation try without valid user!"));
                }

                using (var httpClient = new HttpClient())
                {
                    string jsonString = System.Text.Json.JsonSerializer.Serialize(s);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                    using (var response = await httpClient.PostAsync($"{_restpath}", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<PostVM>(apiResponse);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Creating post failed!");
                return View(ex);
            }

            _logger.LogInformation($"Creating post with title: {result.Title} succeded!");
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
   
    }
}
