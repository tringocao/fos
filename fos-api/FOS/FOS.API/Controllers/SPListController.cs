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
                var accessToken = TokenHelper.GetAccessTokenFromCookie();

                var SiteId = ConfigurationManager.AppSettings["ida:SiteId"];

                //var listId = "3a8b82cb-655b-429c-a774-9a3d2af07289";

                //HttpWebRequest endpointRequest =
                //    (HttpWebRequest)HttpWebRequest.Create(
                //    "https://graph.microsoft.com/v1.0/sites/" + SiteId + "/lists/" + Id);
                //endpointRequest.Method = "GET";
                //endpointRequest.Accept = "application/json;odata=verbose";
                //endpointRequest.Headers.Add("Authorization",
                //  "Bearer " + accessToken);
                //endpointRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                //HttpWebResponse endpointResponse =
                //  (HttpWebResponse)endpointRequest.GetResponse();

                HttpClient client = new HttpClient();

                string path = "https://graph.microsoft.com/v1.0/sites/" + SiteId + "/lists/" + Id;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage responde = await client.SendAsync(request);

                var json = responde.Content.ReadAsStringAsync();
                //var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Content = 

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
