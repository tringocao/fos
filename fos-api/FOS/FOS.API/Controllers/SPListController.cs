using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace FOS.API.Controllers
{
    public class SPListController : ApiController
    {
        // GET api/splist
        //[Authorize]
        public async Task<HttpResponseMessage> Get(string Id)
        {
            var authenticated = TokenHelper.CheckAuthentication();

            if(authenticated == true)
            {
                var accessToken = TokenHelper.GetTokenFromCookie("access_token_key");

                var SiteId = ConfigurationManager.AppSettings["ida:SiteId"];

                HttpClient client = new HttpClient();

                string path = "https://graph.microsoft.com/v1.0/sites/" + SiteId + "/lists/" + Id;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage responde = await client.SendAsync(request);

                var json = responde.Content.ReadAsStringAsync();

                return responde;
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.Moved);
                response.Headers.Location = new Uri(ConfigurationManager.AppSettings["ida:RedirectUri"] + "getauthcode");

                return response;
            }
        }
    }
}
