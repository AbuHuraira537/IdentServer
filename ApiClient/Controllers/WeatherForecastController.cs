using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientFactory factory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IHttpClientFactory factory)
        {
            _logger = logger;
            this.factory = factory;
        }

        [HttpGet]
        public async Task<IActionResult> index()
        {
            var serverClient = factory.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44386/");
            var tokenResponce = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "client_id",
                ClientSecret = "client_serect",
                Scope="ApiOne",
                Resource = { "openid" },

            });
            var apiClient = factory.CreateClient();
            apiClient.SetBearerToken(tokenResponce.AccessToken);
            var responce = await apiClient.GetAsync("https://localhost:44365/secret");
            var content = await responce.Content.ReadAsStringAsync();



            return Ok(new
            {
                access_token = tokenResponce.AccessToken,
                data = content
            });
        }
    }
}
