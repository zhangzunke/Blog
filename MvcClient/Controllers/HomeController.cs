using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MvcClient.Models;
using Newtonsoft.Json;

namespace MvcClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            ViewData["idToken"] = idToken;
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:6001")
            };
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
                );
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            ViewData["accessToken"] = accessToken;
            httpClient.SetBearerToken(accessToken);

            var res = await httpClient.GetAsync("api/posts").ConfigureAwait(false);
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
                var objects = JsonConvert.DeserializeObject<dynamic>(json);
                ViewData["json"] = objects;
                return View();
            }
            if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("AccessDenied", "Authorization");
            }
            throw new Exception($"Error Occurred: ${res.ReasonPhrase}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
