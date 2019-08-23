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
            if (tokenCookie != null)
            {
                var token = MemoryCache.Default.Get(tokenCookie.Value);
                if (token != null)
                {
                    var expireTime = tokenCookie.Expires;
                    //if (expireTime)
                    return true;
                }
            }
            return false;
        }
        public static string GetTokenFromCookie(string tokenType)
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