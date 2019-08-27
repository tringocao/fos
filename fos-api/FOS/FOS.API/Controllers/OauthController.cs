using FOS.Model.Domain;
using FOS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FOS.API.Controllers
{
    public class OauthController : ApiController
    {
        OAuthService _oAuthService;

        public OauthController(OAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }
        [OverrideAuthentication]
        public async Task<HttpResponseMessage> GetAuthCode(string code, string state)
        {
            var redirectUri = "";
            if (code != null)
            {
                var accessToken = await _oAuthService.GetTokenAsync(code);
                redirectUri = JsonConvert.DeserializeObject<State>(state).redirectUri;
            }

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(redirectUri);

            return response;
        }
    }
}
