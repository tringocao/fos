using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using FOS.API.Models;
using System.Runtime.Caching;
using FOS.Common;

namespace FOS.Services
{
    public interface IOAuthService
    {
        string GetAuthCodePath();
        Task<string> GetToken(string code);
        Task<string> RefreshToken();
        Task<bool> CheckAuthentication();
        void SaveToCookie(string accessTokenKey, string refreshTokenKey, int expireDuration);
        string GetTokenKeyFromCookie(string tokenType);
    }

    public class OAuthService : IOAuthService
    {
        public string GetAuthCodePath()
        {
            var tenant = OAuth.TENANT;
            var clientId = OAuth.CLIENT_ID;
            var redirectUri = OAuth.HOME_URI;
            var scope = OAuth.SCOPE;

            var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/v2.0/authorize?" +
                        "client_id=" + clientId +
                        "&response_type=code" +
                        "&redirect_uri=" + redirectUri +
                        "&scope=" + scope;
            return path;
        }

        public async Task<string> GetToken(string code)
        {
            HttpClient client = new HttpClient();

            var tenant = OAuth.TENANT;
            var clientId = OAuth.CLIENT_ID;
            var redirectUri = OAuth.HOME_URI;
            var secret = OAuth.CLIENT_SECRET;

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
            var refreshToken = GetTokenKeyFromCookie("refresh_token");

            HttpClient client = new HttpClient();

            var tenant = OAuth.TENANT;
            var clientId = OAuth.CLIENT_ID;
            var redirectUri = OAuth.HOME_URI;
            var secret = OAuth.CLIENT_SECRET;
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

        public async Task<bool> CheckAuthentication()
        {
            HttpCookie tokenCookie = HttpContext.Current.Request.Cookies["access_token"];

            if (tokenCookie != null)
            {
                Token accessToken = (Token)MemoryCache.Default.Get(tokenCookie.Value);
                if (accessToken != null)
                {
                    if (accessToken._expireTime != null)
                    {
                        if (DateTime.Parse(accessToken._expireTime) < DateTime.Now)
                        {
                            var accessTokenKey = await RefreshToken();
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

            Token accessToken = new Token(accessTokenKey, expireTime);

            MemoryCache.Default.Set(accessTokenId, accessToken, DateTime.Now.AddYears(1));

            HttpCookie accessTokenCookie = new HttpCookie("access_token");

            accessTokenCookie.Value = accessTokenId;
            accessTokenCookie.Expires = DateTime.Now.AddDays(1);

            HttpContext.Current.Response.Cookies.Add(accessTokenCookie);

            var _refreshToken = GetTokenKeyFromCookie("refresh_token");

            if (_refreshToken == null)
            {
                HttpCookie refreshTokenCookie = new HttpCookie("refresh_token");

                string refreshTokenId = Guid.NewGuid().ToString();
                string refreshTokenExpireTime = DateTime.Now.AddYears(1).ToString();

                Token refreshToken = new Token(refreshTokenKey, refreshTokenExpireTime);
                MemoryCache.Default.Set(refreshTokenId, refreshToken, DateTime.Now.AddYears(1));

                refreshTokenCookie.Value = refreshTokenId;
                refreshTokenCookie.Expires = DateTime.Now.AddYears(1);

                HttpContext.Current.Response.Cookies.Add(refreshTokenCookie);
            }
        }

        public string GetTokenKeyFromCookie(string tokenType)
        {
            HttpCookie tokenCookie = HttpContext.Current.Request.Cookies[tokenType];
            if (tokenCookie != null)
            {
                Token token = (Token)MemoryCache.Default.Get(tokenCookie.Value);
                if (token != null)
                {
                    return token._key;
                }
            }
            return null;
        }
    }
}
