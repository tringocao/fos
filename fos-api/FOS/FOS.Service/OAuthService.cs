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
        string LogOut();
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
            var tokenResource = _tokenProvider.GetTokenResourceFromRequest();
            var tokens = tokenResource.tokens;

            Type apiResource = typeof(APIResource);
            foreach (FieldInfo resource in apiResource.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var _resource = resource.GetValue(null).ToString();
                Token token = await GetTokenAsync(tokenResource.RefreshToken, _resource);
                if (token != null)
                {
                    tokens[tokens.FindIndex(_token => _token._resource == _resource)] = token;
                }
            }
            _tokenProvider.SaveTokenResource(tokenResource);

            return tokens.FirstOrDefault()._accessToken;
        }

        public async Task<bool> CheckAuthenticationAsync()
        {
            var tokenResource = _tokenProvider.GetTokenResourceFromRequest();

            if (tokenResource != null)
            {
                Token token = tokenResource.tokens.FirstOrDefault();
                if (token != null)
                {
                    if (DateTime.Compare(DateTime.Parse(token._acessTokenExpireTime), DateTime.Now) < 0)
                    {
                        var accessTokenKey = await RefreshTokenAsync();
                        if (accessTokenKey != null)
                        {
                            // valid token
                            return true;
                        }
                        return false;
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        public string LogOut()
        {
            var tenant = WebConfigurationManager.AppSettings[OAuth.TENANT];
            var redirectUri = WebConfigurationManager.AppSettings[OAuth.HOME_URI];
            var scope = WebConfigurationManager.AppSettings[OAuth.SCOPE];

            var path = "https://login.microsoftonline.com/" + tenant + "/oauth2/logout?" +
                        "post_logout_redirect_uri=" + redirectUri;
            return path;
        }
    }
}
