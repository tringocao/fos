using FOS.API.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FOS.API
{
    public static class TokenHelper
    {
        public static bool CheckAuthentication()
        {
            HttpCookie tokenCookie = HttpContext.Current.Request.Cookies["access_token_key"];
            HttpCookie tokenExpireTime = HttpContext.Current.Request.Cookies["token_expire_time"]; //memorycache

            if (tokenCookie != null)
            {
                var token = MemoryCache.Default.Get(tokenCookie.Value); //also expire time in one object
                if (token != null)
                {
                    if (tokenExpireTime != null)
                    {
                        if (DateTime.Parse(tokenExpireTime.Value) < DateTime.Now)
                        {
                            var accessToken = new OAuthController().RefreshToken();
                            if (accessToken != null)
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

        public static string GetTokenFromCookie(string tokenType) //get key from token
        {
            HttpCookie tokenCookie = HttpContext.Current.Request.Cookies[tokenType];
            if (tokenCookie != null)
            {
                var token = MemoryCache.Default.Get(tokenCookie.Value);
                if (token != null)
                {
                    return token.ToString();
                }
            }
            return null;
        }
    }
}