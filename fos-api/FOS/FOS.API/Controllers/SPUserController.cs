using FOS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace FOS.API.Controllers
{
    public class SPUserController : ApiController
    {
        IOAuthService _oAuthService;

        public SPUserController(IOAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        // GET api/spuser/getusers
        public async Task<HttpResponseMessage> GetUsers()
        {
            var accessToken = _oAuthService.GetTokenFromCookie()._accessToken;

            HttpClient client = new HttpClient();

            string path = "https://graph.microsoft.com/v1.0/users";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage responde = await client.SendAsync(request);

            var json = responde.Content.ReadAsStringAsync();

            return responde;
        }
    }
}
