using FOS.API.App_Start;
using FOS.API.Models;
using FOS.Common;
using FOS.Model.Domain;
using FOS.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    public class OauthController : ApiController
    {
        OAuthService _oAuthService;

        public OauthController(OAuthService oAuthService, ConfigurationModel configuraModel)
        {
            _oAuthService = oAuthService;
        }
        [OverrideAuthentication]
        public async Task<HttpResponseMessage> GetAuthCode(string code, string state)
        {
            var redirectUri = "";
            if (code != null)
            {
                await _oAuthService.SaveTokensAsync(code);
                redirectUri = JsonConvert.DeserializeObject<State>(state).redirectUri;
            }

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(redirectUri);

            return response;
        }
        // GET: api/oauth/checkauth
        [HttpGet]
        [OverrideAuthentication]
        public async Task<HttpResponseMessage> CheckAuth()
        {
            var authenticated = await _oAuthService.CheckAuthenticationAsync();
            var response = new HttpResponseMessage();

            response.Content = new ObjectContent<AuthClientRespond>(
                new AuthClientRespond()
                {
                    redirect = !authenticated,
                    redirectUrl = _oAuthService.GetAuthCodePath(new State(
                        WebConfigurationManager.AppSettings[OAuth.HOME_URI]
                    ))
                }, new JsonMediaTypeFormatter(), "application/json"
            );

            return response;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> LogOut()
        {
            var response = new HttpResponseMessage();

            response.Content = new ObjectContent<AuthClientRespond>(
                new AuthClientRespond()
                {
                    redirect = true,
                    redirectUrl = _oAuthService.LogOut()
                }, new JsonMediaTypeFormatter(), "application/json"
            );

            foreach (string cookie in HttpContext.Current.Request.Cookies.AllKeys) { HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1); }


            return response;
        }
    }
}
