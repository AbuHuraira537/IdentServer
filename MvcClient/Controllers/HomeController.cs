using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcClient.Models;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory factory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            this.factory = factory;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var access_token = await HttpContext.GetTokenAsync("access_token");
            var id_token = await HttpContext.GetTokenAsync("id_token");
            var refresh_token = await HttpContext.GetTokenAsync("refresh_token");

          await  RefreshToken();
            var access = new JwtSecurityTokenHandler().ReadJwtToken(access_token);
            var id = new JwtSecurityTokenHandler().ReadJwtToken(id_token);
            string sec = await GetSecret(access_token);
            return View();
        }
    
        public async Task<string> GetSecret(string accessToken)
        {
            var claims =
                 User.Claims.ToList();
            var apiClient = factory.CreateClient();
            apiClient.SetBearerToken(accessToken);
            var responce = await apiClient.GetAsync("https://localhost:44365/WeatherForecast");
            var content = await responce.Content.ReadAsStringAsync();

            return content;

           
        }
        public async Task RefreshToken()
        {
            var serverClient = factory.CreateClient();
            var discoverDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44386/");
            
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            var refreshTokenClient = factory.CreateClient();

            var tokenResponce = await refreshTokenClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                ClientId = "client_mvc",
                ClientSecret = "client_mvc_secret",
                Address = discoverDocument.TokenEndpoint,
                RefreshToken=refreshToken,
            });
            var authInfo = await HttpContext.AuthenticateAsync("Cookie");
            //when you get new token you must update here;
            //authInfo.Properties.UpdateTokenValue("")
            //authInfo.Properties.UpdateTokenValue()


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
