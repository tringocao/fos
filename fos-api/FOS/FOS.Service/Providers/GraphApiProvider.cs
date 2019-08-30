using FOS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace FOS.Services.Providers
{
    public class GraphApiProvider : IGraphApiProvider
    {
        ITokenProvider _tokenProvider;
        public GraphApiProvider(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string endPoint, string data)
        {
            var SiteId = WebConfigurationManager.AppSettings[OAuth.SITE_ID];
            HttpClient client = new HttpClient();

            var resource = "https://graph.microsoft.com";

            var accessToken = _tokenProvider.GetTokenByResourceUrl(resource)._accessToken;

            string path = resource + "/v1.0/" + endPoint;

            HttpRequestMessage request = new HttpRequestMessage(method, path);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (data != null)
            {
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage responde = await client.SendAsync(request);

            return responde;
        }
    }
}
