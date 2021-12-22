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

namespace SocialMedia.WebApp.Controllers
{
    public class PostController : Controller
    {
        public IConfiguration Configuration;

        public PostController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ContentResult GetHostUrl()
        {
            var result = Configuration["RestApiUrl:HostUrl"];
            return Content(result);
        }

        private string CN()
        {
            string cn = ControllerContext.RouteData.Values["controller"].ToString();
            return cn;
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperTajneHaslo2137"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Name", "Marek"),
                new Claim(JwtRegisteredClaimNames.Email, "01153053@pw.edu.pl")
            };

            var token = new JwtSecurityToken(
                issuer: "http://www.nowakom3.pl",
                audience: "http://www.nowakom3.pl",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: credentials,
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IActionResult> Index()
        {
            var tokenString = GenerateJSONWebToken();

            //string _restpath = "http://localhost:5000/skijumper";
            string _restpath = GetHostUrl().Content + CN();

            List<PostVM> skiJumpersList = new List<PostVM>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync(_restpath))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    skiJumpersList = JsonConvert.DeserializeObject<List<PostVM>>(apiResponse);
                }
            }

            return View(skiJumpersList);    //view is strongly typed
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tokenString = GenerateJSONWebToken();
            string _restpath = GetHostUrl().Content + CN();

            PostVM s = new PostVM();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.GetAsync($"{_restpath}/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    s = JsonConvert.DeserializeObject<PostVM>(apiResponse);
                }
            }

            return View(s);    //view is strongly typed
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostVM s)
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            PostVM result = new PostVM();

            try
            {
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
                return View(ex);
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpClient.DeleteAsync($"{_restpath}/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction(nameof(Index));    //view is strongly typed
        }

        public async Task<IActionResult> Create()
        {
            PostVM s = new PostVM();
            return await Task.FromResult(View(s));    //view is strongly typed
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostVM s)
        {
            var tokenString = GenerateJSONWebToken();

            string _restpath = GetHostUrl().Content + CN();

            PostVM result = new PostVM();

            try
            {
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
                return View(ex);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
