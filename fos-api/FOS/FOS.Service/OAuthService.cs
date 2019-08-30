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
using FOS.Services.Providers;
using System.Reflection;

namespace FOS.Services
{
    public interface IOAuthService
    {
        string GetAuthCodePath(State state);
        Task SaveTokensAsync(string code);
        Task<string> RefreshTokenAsync();
        Task<bool> CheckAuthenticationAsync();
    }

    public class OAuthService : IOAuthService
    {
        ITokenProvider _tokenProvider;

        public OAuthService(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public string GetAuthCodePath(State _state)
        {
            var tenant = WebConfigurationManager.AppSettings[OAuth.TENANT];
            var clientId = WebConfigurationManager.AppSettings[OAuth.CLIENT_ID];
            var redirectUri = WebConfigurationManager.AppSettings[OAuth.REDIRECT_URI];
            var scope = WebConfigurationManager.AppSettings[OAuth.SCOPE];
            var state = new JavaScriptSerializer().Serialize(_state);

            var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/authorize?" +
                        "client_id=" + clientId +
                        "&response_type=code" +
                        "&redirect_uri=" + redirectUri +
                        "&scope=" + scope +
                        "&state=" + state;
            return path;
        }

        public async Task SaveTokensAsync(string code)
        {
            HttpClient client = new HttpClient();

            var tenant = WebConfigurationManager.AppSettings[OAuth.TENANT];
            var clientId = WebConfigurationManager.AppSettings[OAuth.CLIENT_ID];
            var redirectUri = WebConfigurationManager.AppSettings[OAuth.REDIRECT_URI];
            var secret = WebConfigurationManager.AppSettings[OAuth.CLIENT_SECRET];

            var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/token";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("redirect_uri", redirectUri), // back to previous url
                new KeyValuePair<string, string>("client_secret", secret),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("resource", APIResource.SHAREPOINT_CONTEXT),
                new KeyValuePair<string, string>("code", code),
            });

            HttpResponseMessage response = await client.PostAsync(path, content);
            var resultContent2 = await response.Content.ReadAsStringAsync();
            var resultContent = await response.Content.ReadAsAsync<OAuthResponse>();

            TokenResource tokenResource = new TokenResource()
            {
                RefreshToken = resultContent.refresh_token,
                tokens = new List<Token>(),
            };
            tokenResource.tokens.Add(new Token()
            {
                _accessToken = resultContent.access_token,
                _resource = APIResource.SHAREPOINT_CONTEXT,
                _acessTokenExpireTime = DateTime.Now.AddSeconds(resultContent.expires_in).ToString(),
            });

            // add other resource tokens to tokenResource

            Type apiResource = typeof(APIResource);
            foreach (FieldInfo resource in apiResource.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var _resource = resource.GetValue(null).ToString();
                if (_resource!= APIResource.SHAREPOINT_CONTEXT)
                {
                    Token token = await GetTokenAsync(resultContent.refresh_token, _resource);
                    tokenResource.tokens.Add(token);
                }
            }
            _tokenProvider.SaveTokenResource(tokenResource);
            
        }

        private async Task<Token> GetTokenAsync(string token, string resourceUrl)
        {
            HttpClient client = new HttpClient();

            var tenant = WebConfigurationManager.AppSettings[OAuth.TENANT];
            var clientId = WebConfigurationManager.AppSettings[OAuth.CLIENT_ID];
            var secret = WebConfigurationManager.AppSettings[OAuth.CLIENT_SECRET];
            var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/token";
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_secret", secret),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("resource", resourceUrl),
                new KeyValuePair<string, string>("refresh_token", token),
            });

            HttpResponseMessage response = await client.PostAsync(path, content);
            var resultContentString = await response.Content.ReadAsStringAsync();
            var resultContent = await response.Content.ReadAsAsync<OAuthResponse>();
            //return resultContent;
            if (resultContent.access_token != null)
            {
                Token _token = new Token()
                {
                    _accessToken = resultContent.access_token,
                    _acessTokenExpireTime = DateTime.Now.AddSeconds(resultContent.expires_in).ToString(),
                    _resource = resourceUrl
                };
                return _token;
            }
            return null;
        }

        public async Task<string> RefreshTokenAsync()
        {
            var refreshToken = _tokenProvider.GetTokenResourceFromRequest().RefreshToken;

            HttpClient client = new HttpClient();

            var tenant = WebConfigurationManager.AppSettings[OAuth.TENANT];
            var clientId = WebConfigurationManager.AppSettings[OAuth.CLIENT_ID];
            var secret = WebConfigurationManager.AppSettings[OAuth.CLIENT_SECRET];
            var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/token";
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
                //SaveToCookie(resultContent.access_token, resultContent.refresh_token, resultContent.expires_in);
            }
            return resultContent.access_token;
        }

        public async Task<bool> CheckAuthenticationAsync()
        {
            //string requestUrl = HttpContext.Current.Request.Url.ToString();

            var tokenResource = _tokenProvider.GetTokenResourceFromRequest();

            if (tokenResource != null)
            {
                return true;
            }
            return false;
            //var tokenCookie = HttpContext.Current.Request.Headers.GetValues("token_key");

            //if (tokenCookie != null)
            //{
            //    Token token = (Token)MemoryCache.Default.Get(tokenCookie.FirstOrDefault());
            //    if (token != null)
            //    {
            //        if (token._acessTokenExpireTime != null)
            //        {
            //            if (DateTime.Parse(token._acessTokenExpireTime) < DateTime.Now)
            //            {
            //                var accessTokenKey = await RefreshTokenAsync();
            //                if (accessTokenKey != null)
            //                {
            //                    // valid token
            //                    return true;
            //                }
            //            }
            //            return true;
            //        }
            //    }
            //}
        }

        //public void SaveToCookie(string accessTokenKey, string refreshTokenKey, int expireDuration)
        //{
        //    string accessTokenId = Guid.NewGuid().ToString();
        //    string expireTime = DateTime.Now.AddSeconds(expireDuration).ToString();

        //    var _token = GetTokenFromCookie();

        //    if (_token != null)
        //    {
        //        _token._accessToken = accessTokenKey;
        //    }
        //    else
        //    {
        //        _token = new Token(accessTokenKey, refreshTokenKey, expireTime);
        //    }

        //    MemoryCache.Default.Set(accessTokenId, _token, DateTime.Now.AddYears(1));

        //    HttpCookie tokenCookie = new HttpCookie("token_key");

        //    tokenCookie.Value = accessTokenId;
        //    tokenCookie.Expires = DateTime.Now.AddDays(1);

        //    HttpContext.Current.Response.Cookies.Add(tokenCookie);
        //}

        //public Token GetTokenFromCookie()
        //{
        //    var abc = HttpContext.Current.Request;
        //    HttpCookie tokenCookieFromRequest = HttpContext.Current.Request.Cookies["token_key"];
        //    var tokenCookieFromHeader = HttpContext.Current.Request.Headers.GetValues("token_key");

        //    if (tokenCookieFromRequest != null)
        //    {
        //        Token token = (Token)MemoryCache.Default.Get(tokenCookieFromRequest.Value);
        //        if (token != null)
        //        {
        //            return token;
        //        }
        //    }
        //    else if (tokenCookieFromHeader != null)
        //    {
        //        Token token = (Token)MemoryCache.Default.Get(tokenCookieFromHeader.FirstOrDefault());
        //        if (token != null)
        //        {
        //            return token;
        //        }
        //    }
        //    return null;
        //}
    }
}
