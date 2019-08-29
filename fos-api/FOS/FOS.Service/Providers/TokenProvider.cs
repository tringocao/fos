using FOS.API.Models;
using FOS.Common;
using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FOS.Services.Providers
{
    public class TokenProvider : ITokenProvider
    {
        public Token GetTokenByResourceUrl(string resourceUrl)
        {
            TokenResource tokenResource = GetTokenResourceFromRequest();
            if (tokenResource != null)
            {
                return tokenResource.tokens.SingleOrDefault(_token => _token._resource == resourceUrl);
            }
            return null;
        }

        public TokenResource GetTokenResourceFromRequest()
        {
            HttpCookie tokenCookieFromRequest = HttpContext.Current.Request.Cookies["token_key"];
            var tokenCookieFromHeader = HttpContext.Current.Request.Headers.GetValues("token_key");

            if (tokenCookieFromRequest != null)
            {
                TokenResource token = (TokenResource)MemoryCache.Default.Get(tokenCookieFromRequest.Value);
                if (token != null)
                {
                    return token;
                }
            }
            else if (tokenCookieFromHeader != null)
            {
                TokenResource token = (TokenResource)MemoryCache.Default.Get(tokenCookieFromHeader.FirstOrDefault());
                if (token != null)
                {
                    return token;
                }
            }
            return null;
        }

        public void SaveTokenResource(TokenResource tokenResource)
        {
            string accessTokenId = Guid.NewGuid().ToString();

            MemoryCache.Default.Set(accessTokenId, tokenResource, DateTime.Now.AddYears(1));

            HttpCookie tokenCookie = new HttpCookie("token_key");

            tokenCookie.Value = accessTokenId;
            tokenCookie.Expires = DateTime.Now.AddDays(1);

            HttpContext.Current.Response.Cookies.Add(tokenCookie);

            //string expireTime = DateTime.Now.AddSeconds(expireDuration).ToString();

            //var _token = GetTokenByResourceUrl(resourceUrl);


            //var _token = GetTokenByResourceUrl(resourceUrl);

            //if (_token != null)
            //{
            //    _token._accessToken = token._accessToken;
            //}
            //else
            //{
            //    _token = token;
            //}

            //TokenResource tokenResource = GetTokenResourceFromRequest();
            //if (tokenResource != null)
            //{
            //    if (resourceUrl == APIResource.GRAPH_API)
            //    {
            //        tokenResource.graphApiToken = token;
            //    }
            //    else if (resourceUrl == APIResource.SHAREPOINT_CONTEXT)
            //    {
            //        tokenResource.sharepointToken = token;
            //    }
            //}
            //else
            //{
            //    if (resourceUrl == APIResource.GRAPH_API)
            //    {
            //        tokenResource = new TokenResource() { graphApiToken = token };
            //    }
            //    else if (resourceUrl == APIResource.SHAREPOINT_CONTEXT)
            //    {
            //        tokenResource = new TokenResource() { sharepointToken = token };
            //    }
            //}
        }
    }
}
