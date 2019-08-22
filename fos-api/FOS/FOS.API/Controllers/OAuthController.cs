using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using FOS.API.Models;
using System.Web.Caching;
using System.Runtime.Caching;

namespace FOS.API.Controllers
{
    public class OAuthController : Controller
    {

        public async Task<ActionResult> Index()
        {
            string url = Request.Url.AbsoluteUri;
            var redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

            var code = Request.QueryString["code"];

            if (code != null)
            {
                var accessToken = await GetToken(code);

            }

            return Redirect(ConfigurationManager.AppSettings["ida:HomeUri"]);
        }

        public async Task<ActionResult> GetAuthCode()
        {
            //var accessToken = GetAccessTokenFromCookie();
            //if (accessToken == null)
            //{
                var tenant = ConfigurationManager.AppSettings["ida:Tenant"];
                var clientId = ConfigurationManager.AppSettings["ida:ClientId"];
                var redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
                var scope = "Sites.FullControl.All";

                var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/v2.0/authorize?" +
                            "client_id=" + clientId +
                            "&response_type=code" +
                            "&redirect_uri=" + redirectUri +
                            "&scope=" + scope +
                            "&state=12345";
                return Redirect(path);
            //}
            //else
            //{
            //    return Redirect(ConfigurationManager.AppSettings["ida:HomeUri"]);
            //}
        }

        public async Task<string> GetToken(string code)
        {
            HttpClient client = new HttpClient();

            var tenant = ConfigurationManager.AppSettings["ida:Tenant"];
            var clientId = ConfigurationManager.AppSettings["ida:ClientId"];
            var redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
            var secret = ConfigurationManager.AppSettings["ida:ClientSecret"];
            var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/v2.0/token";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("client_secret", secret),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
            });
            HttpResponseMessage response = await client.PostAsync(path, content);
            //var resultContent = await response.Content.ReadAsStringAsync();
            var resultContent = await response.Content.ReadAsAsync<OAuthResponse>();
            //return resultContent;

            SaveToCookie(resultContent.access_token, resultContent.expires_in);
            return resultContent.access_token;
        }

        private void SaveToCookie(string access_token, int expireDuration)
        {
            HttpCookie tokenCookie = new HttpCookie("access_token_key");
            DateTime now = DateTime.Now;

            var policy = now.AddSeconds(expireDuration);

            MemoryCache.Default.Add(policy.ToString(), access_token, policy);

            tokenCookie.Value = policy.ToString();

            tokenCookie.Expires = now.AddSeconds(expireDuration);

            Response.Cookies.Add(tokenCookie);
        }

        public ActionResult CheckAuthentication()
        {
            HttpCookie tokenCookie = Request.Cookies["access_token_key"];
            if (tokenCookie != null)
            {
                var token = MemoryCache.Default.Get(tokenCookie.Value);
                if (token != null)
                {
                    // valid token
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            return Redirect(ConfigurationManager.AppSettings["ida:RedirectUri"] + "getauthcode");
        }

        public string GetAccessTokenFromCookie()
        {
            HttpCookie tokenCookie = Request.Cookies["access_token_key"];
            if (tokenCookie != null)
            {
                var token = MemoryCache.Default.Get(tokenCookie.Value);
                if (token !=null)
                {
                    return token.ToString();
                }
            }
            return null;
        }
    }
}