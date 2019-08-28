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
        IGraphHttpClient _graphHttpClient;

        public SPListController(IOAuthService oAuthService, IGraphHttpClient graphHttpClient)
        {
            _oAuthService = oAuthService;
            _graphHttpClient = graphHttpClient;
        }
        // GET api/splist/getlist/{list-id}
        public async Task<HttpResponseMessage> GetList(string Id)
        {
            var SiteId = WebConfigurationManager.AppSettings[OAuth.SITE_ID];

            HttpClient client = new HttpClient();

            string path = "https://graph.microsoft.com/v1.0/sites/" + SiteId + "/lists/" + Id;
            HttpRequestMessage request = _graphHttpClient.GetRequestMessage(path, HttpMethod.Get);
            HttpResponseMessage responde = await client.SendAsync(request);

            var json = responde.Content.ReadAsStringAsync();

            return responde;
        }
        // POST api/splist/addlistitem/{list-id}/
        public async Task<HttpResponseMessage> AddListItem(string Id, [FromBody]dynamic item)
        {
            var SiteId = WebConfigurationManager.AppSettings[OAuth.SITE_ID];
            HttpClient client = new HttpClient();

            string path = "https://graph.microsoft.com/v1.0/sites/" + SiteId + "/lists/" + Id + "/items";
            HttpRequestMessage request = _graphHttpClient.GetRequestMessage(path, HttpMethod.Post);

            request.Content = new StringContent( item, Encoding.UTF8, "application/json");

            HttpResponseMessage responde = await client.SendAsync(request);

            var json = responde.Content.ReadAsStringAsync();

            return responde;
        }
    }
}
