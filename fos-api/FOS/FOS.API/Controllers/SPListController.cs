using FOS.Common;
using FOS.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        // GET api/splist
        public async Task<HttpResponseMessage> Get(string Id)
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
    }
}
