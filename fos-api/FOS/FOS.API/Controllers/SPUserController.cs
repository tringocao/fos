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
        IGraphHttpClient _graphHttpClient;

        public SPUserController(IOAuthService oAuthService, IGraphHttpClient graphHttpClient)
        {
            _oAuthService = oAuthService;
            _graphHttpClient = graphHttpClient;
        }

        // GET api/spuser/getusers
        public async Task<HttpResponseMessage> GetUsers()
        {
            HttpClient client = new HttpClient();

            string path = "https://graph.microsoft.com/v1.0/users";
            HttpRequestMessage request = _graphHttpClient.GetRequestMessage(path, HttpMethod.Get);

            HttpResponseMessage responde = await client.SendAsync(request);

            var json = responde.Content.ReadAsStringAsync();

            return responde;
        }

        // GET api/spuser/GetCurrentUser
        public async Task<HttpResponseMessage> GetCurrentUser()
        {
            HttpClient client = new HttpClient();

            string path = "https://graph.microsoft.com/v1.0/me";
            HttpRequestMessage request = _graphHttpClient.GetRequestMessage(path, HttpMethod.Get);

            HttpResponseMessage responde = await client.SendAsync(request);

            var json = responde.Content.ReadAsStringAsync();

            return responde;
        }

        // GET api/spuser/GetUserById/Id
        public async Task<HttpResponseMessage> GetUserById(string Id)
        {
            HttpClient client = new HttpClient();

            string path = "https://graph.microsoft.com/v1.0/users/" + Id;
            HttpRequestMessage request = _graphHttpClient.GetRequestMessage(path, HttpMethod.Get);

            HttpResponseMessage responde = await client.SendAsync(request);

            var json = responde.Content.ReadAsStringAsync();

            return responde;
        }
    }
}
