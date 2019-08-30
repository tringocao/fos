using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web;
using FOS.API.Models;
using System.Runtime.Caching;
using FOS.Common;
using FOS.Model.Domain;
using System.Web.Script.Serialization;

namespace FOS.Services
{
    public interface IOAuthService
    {
        string GetAuthCodePath(State state);
        Task<string> GetTokenAsync(string code);
        Task<string> RefreshTokenAsync();
        Task<bool> CheckAuthenticationAsync();
        void SaveToCookie(string accessTokenKey, string refreshTokenKey, int expireDuration);
        Token GetTokenFromCookie();
    }

    public class OAuthService : IOAuthService
    {

        public string GetAuthCodePath(State _state)
        {
            var tenant = WebConfigurationManager.AppSettings[OAuth.TENANT];
            var clientId = WebConfigurationManager.AppSettings[OAuth.CLIENT_ID];
            var redirectUri = WebConfigurationManager.AppSettings[OAuth.REDIRECT_URI];
            var scope = WebConfigurationManager.AppSettings[OAuth.SCOPE];
            var state = new JavaScriptSerializer().Serialize(_state);

            var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/v2.0/authorize?" +
                        "client_id=" + clientId +
                        "&response_type=code" +
                        "&redirect_uri=" + redirectUri +
                        "&scope=" + scope +
                        "&state=" + state;
            return path;
        }

        public async Task<string> GetTokenAsync(string code)
        {
            HttpClient client = new HttpClient();

            var tenant = WebConfigurationManager.AppSettings[OAuth.TENANT];
            var clientId = WebConfigurationManager.AppSettings[OAuth.CLIENT_ID];
            var redirectUri = WebConfigurationManager.AppSettings[OAuth.REDIRECT_URI];
            var secret = WebConfigurationManager.AppSettings[OAuth.CLIENT_SECRET];

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
            //var resultContent2 = await response.Content.ReadAsStringAsync();
            var resultContent = await response.Content.ReadAsAsync<OAuthResponse>();
            //return resultContent;
            if (resultContent.access_token != null && resultContent.refresh_token != null)
            {
                SaveToCookie(resultContent.access_token, resultContent.refresh_token, resultContent.expires_in);
            }
            return resultContent.access_token;
        }

        public async Task<string> RefreshTokenAsync()
        {
            var refreshToken = GetTokenFromCookie()._refreshToken;

            HttpClient client = new HttpClient();

            var tenant = WebConfigurationManager.AppSettings[OAuth.TENANT];
            var clientId = WebConfigurationManager.AppSettings[OAuth.CLIENT_ID];
            var secret = WebConfigurationManager.AppSettings[OAuth.CLIENT_SECRET];
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
                SaveToCookie(resultContent.access_token, resultContent.refresh_token, resultContent.expires_in);
            }
            return resultContent.access_token;
        }

        public async Task<bool> CheckAuthenticationAsync()
        {
            var tokenCookie = HttpContext.Current.Request.Headers.GetValues("token_key");

            if (tokenCookie != null)
            {
                Token token = (Token)MemoryCache.Default.Get(tokenCookie.FirstOrDefault());
                if (token != null)
                {
                    if (token._acessTokenExpireTime != null)
                    {
                        if (DateTime.Parse(token._acessTokenExpireTime) < DateTime.Now)
                        {
                            var accessTokenKey = await RefreshTokenAsync();
                            if (accessTokenKey != null)
                            {
                                // valid token
                                return true;
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public void SaveToCookie(string accessTokenKey, string refreshTokenKey, int expireDuration)
        {
            string accessTokenId = Guid.NewGuid().ToString();
            string expireTime = DateTime.Now.AddSeconds(expireDuration).ToString();

            var _token = GetTokenFromCookie();

            if (_token != null)
            {
                _token._accessToken = accessTokenKey;
            }
            else
            {
                _token = new Token(accessTokenKey, refreshTokenKey, expireTime);
            }

            MemoryCache.Default.Set(accessTokenId, _token, DateTime.Now.AddYears(1));

            HttpCookie tokenCookie = new HttpCookie("token_key");

            tokenCookie.Value = accessTokenId;
            tokenCookie.Expires = DateTime.Now.AddDays(1);

            HttpContext.Current.Response.Cookies.Add(tokenCookie);

            //if (_refreshToken == null)
            //{
            //    HttpCookie refreshTokenCookie = new HttpCookie("refresh_token");

            //    string refreshTokenId = Guid.NewGuid().ToString();
            //    string refreshTokenExpireTime = DateTime.Now.AddYears(1).ToString();

            //    Token refreshToken = new Token(refreshTokenKey, refreshTokenExpireTime);
            //    MemoryCache.Default.Set(refreshTokenId, refreshToken, DateTime.Now.AddYears(1));

            //    refreshTokenCookie.Value = refreshTokenId;
            //    refreshTokenCookie.Expires = DateTime.Now.AddYears(1);

            //    HttpContext.Current.Response.Cookies.Add(refreshTokenCookie);
            //}
        }

        public Token GetTokenFromCookie()
        {
            var abc = HttpContext.Current.Request;
            HttpCookie tokenCookieFromRequest = HttpContext.Current.Request.Cookies["token_key"];
            var tokenCookieFromHeader = HttpContext.Current.Request.Headers.GetValues("token_key");

            if (tokenCookieFromRequest != null)
            {
                Token token = (Token)MemoryCache.Default.Get(tokenCookieFromRequest.Value);
                if (token != null)
                {
                    return token;
                }
            }
            else if (tokenCookieFromHeader != null)
            {
                Token token = (Token)MemoryCache.Default.Get(tokenCookieFromHeader.FirstOrDefault());
                if (token != null)
                {
                    return token;
                }
            }
            return null;
        }
    }
}
