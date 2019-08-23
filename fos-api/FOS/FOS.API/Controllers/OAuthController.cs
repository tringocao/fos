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
            var tenant = ConfigurationManager.AppSettings["ida:Tenant"];
            var clientId = ConfigurationManager.AppSettings["ida:ClientId"];
            var redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
            var scope = ConfigurationManager.AppSettings["ida:Scope"];

            var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/v2.0/authorize?" +
                        "client_id=" + clientId +
                        "&response_type=code" +
                        "&redirect_uri=" + redirectUri +
                        "&scope=" + scope +
                        "&state=12345";
            return Redirect(path);
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
            if (resultContent.access_token != null && resultContent.refresh_token != null)
            {
                SaveToCookie(resultContent.access_token, resultContent.refresh_token, resultContent.expires_in);
            }
            return resultContent.access_token;
        }

        public async Task<string> RefreshToken()
        {
            var refreshToken = TokenHelper.GetTokenFromCookie("refresh_token_key");

            HttpClient client = new HttpClient();

            var tenant = ConfigurationManager.AppSettings["ida:Tenant"];
            var clientId = ConfigurationManager.AppSettings["ida:ClientId"];
            var redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
            var secret = ConfigurationManager.AppSettings["ida:ClientSecret"];
            var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/v2.0/token";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_secret", secret),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
            });
            HttpResponseMessage response = await client.PostAsync(path, content);
            var resultContentString = await response.Content.ReadAsStringAsync();
            var resultContent = await response.Content.ReadAsAsync<OAuthResponse>();
            //return resultContent;
            if (resultContent.access_token != null && resultContent.refresh_token != null)
            {
                SaveToCookie(resultContent.access_token, resultContent.access_token, resultContent.expires_in);
            }            
            return resultContent.access_token;
        }

        private void SaveToCookie(string access_token, string refresh_token, int expireDuration)
        {
            HttpCookie accessTokenCookie = new HttpCookie("access_token_key");
            DateTime now = DateTime.Now;
            var policy = now.AddSeconds(expireDuration);

            accessTokenCookie.Value = "access_" + policy.ToString();
            accessTokenCookie.Expires = now.AddSeconds(expireDuration);

            Response.Cookies.Add(accessTokenCookie);

            MemoryCache.Default.Set("access_" + policy.ToString(), access_token, policy);

            var refreshToken = TokenHelper.GetTokenFromCookie("refresh_token_key");

            if (refreshToken == null)
            {
                HttpCookie refreshTokenCookie = new HttpCookie("refresh_token_key");
                refreshTokenCookie.Value = "refresh_" + policy.ToString();
                refreshTokenCookie.Expires = now.AddYears(99);

                Response.Cookies.Add(refreshTokenCookie);

                MemoryCache.Default.Set("refresh_" + policy.ToString(), refresh_token, policy);
            }           
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
    }
}