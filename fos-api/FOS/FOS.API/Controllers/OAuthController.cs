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

            // ensure success responde before access code
            var code = Request.QueryString["code"];

            if (code != null)
            {
                var accessToken = await GetToken(code);

            }

            return Redirect(ConfigurationManager.AppSettings["ida:HomeUri"]); // 401 
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
                        "&scope=" + scope;
                        //"&state=12345";
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
                new KeyValuePair<string, string>("redirect_uri", redirectUri), // back to previous url
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
            AccessToken accessToken = new AccessToken()


            HttpCookie accessTokenCookie = new HttpCookie("access_token_key");
            //HttpCookie accessTokenExpireTimeCookie = new HttpCookie("token_expire_time"); /// one object
            DateTime now = DateTime.Now;
            var policy = now.AddSeconds(expireDuration);
            var accessTokenId = Guid.NewGuid().ToString();
            accessTokenCookie.Value = accessTokenId;
            accessTokenCookie.Expires = now.AddDays(1);

            //var expireTime = now.AddSeconds(expireDuration);
            //accessTokenExpireTimeCookie.Value = policy.ToString();

            Response.Cookies.Add(accessTokenCookie);
            //Response.Cookies.Add(accessTokenExpireTimeCookie);

            MemoryCache.Default.Set(accessTokenId, access_token, now.AddDays(1));

            var refreshToken = TokenHelper.GetTokenFromCookie("refresh_token_key");

            if (refreshToken == null)
            {
                HttpCookie refreshTokenCookie = new HttpCookie("refresh_token_key");
                refreshTokenCookie.Value = "refresh_" + policy.ToString();
                refreshTokenCookie.Expires = now.AddYears(1);

                Response.Cookies.Add(refreshTokenCookie);

                MemoryCache.Default.Set("refresh_" + policy.ToString(), refresh_token, now.AddYears(1));
            }           
        }

        //public async Task<ActionResult> CheckAuthentication()
        //{
        //    HttpCookie tokenCookie = Request.Cookies["access_token_key"];
        //    HttpCookie tokenExpireTime = Request.Cookies["token_expire_time"];

        //    if (tokenCookie != null)
        //    {
        //        var token = MemoryCache.Default.Get(tokenCookie.Value);
        //        if (token != null)
        //        {
        //            if (tokenExpireTime != null)
        //            {
        //                if (DateTime.Parse(tokenExpireTime.Value) < DateTime.Now)
        //                {
        //                    var accessToken = await RefreshToken();
        //                    if (accessToken != null)
        //                    {
        //                        // valid token
        //                        return Redirect(Request.UrlReferrer.ToString());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return Redirect(ConfigurationManager.AppSettings["ida:RedirectUri"] + "getauthcode");
        //}
    }
}