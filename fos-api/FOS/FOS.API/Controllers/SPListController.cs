using FOS.Common;
using FOS.Model.Domain;
using FOS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace FOS.API.Controllers
{
    public class SPListController : ApiController
    {
        IOAuthService _oAuthService;

        public SPListController(IOAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }
        // GET api/splist/getlist/{list-id}
        public async Task<HttpResponseMessage> GetList(string Id)
        {
            var accessToken = _oAuthService.GetTokenFromCookie()._accessToken;

            var SiteId = WebConfigurationManager.AppSettings[OAuth.SITE_ID];

            HttpClient client = new HttpClient();

            string path = "https://graph.microsoft.com/v1.0/sites/" + SiteId + "/lists/" + Id;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage responde = await client.SendAsync(request);

            var json = responde.Content.ReadAsStringAsync();

            return responde;
        }
        // POST api/splist/addlistitem/{list-id}/
        public async Task<HttpResponseMessage> AddListItem(string Id)
        {
            var accessToken = _oAuthService.GetTokenFromCookie()._accessToken;

            var SiteId = WebConfigurationManager.AppSettings[OAuth.SITE_ID];

            HttpClient client = new HttpClient();

            string path = "https://graph.microsoft.com/v1.0/sites/" + SiteId + "/lists/" + Id + "/items";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, path);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            request.Content = new StringContent("{\"fields\":" + 
                JsonConvert.SerializeObject(new Event("event 1", "1")) + 
                "}", Encoding.UTF8, "application/json");


            HttpResponseMessage responde = await client.SendAsync(request);

            var json = responde.Content.ReadAsStringAsync();

            return responde;
        }
    }
}
