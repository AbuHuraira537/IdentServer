using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiClient.Controllers
{
    [Route("controller")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHttpClientFactory factory;

        public HomeController(IHttpClientFactory factory)
        {
            this.factory = factory;
        }
        [HttpGet]
        public async Task<IActionResult> index()
        {
            var serverClient = factory.CreateClient();
            var discoveryDocument =await serverClient.GetDiscoveryDocumentAsync("https://localhost:44386/");
            var tokenResponce = await serverClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId= "client_id",
                ClientSecret= "client_serect",
                Scope="all",

            });
            var apiClient = factory.CreateClient();
            apiClient.SetBearerToken(tokenResponce.AccessToken);
            var responce =await apiClient.GetAsync("https://localhost:44365/WeatherForecast/secret");
            var content =await responce.Content.ReadAsStringAsync();
            


            return Ok(new
            {access_token=tokenResponce.AccessToken,
            data=content
            });
        }
    }
}