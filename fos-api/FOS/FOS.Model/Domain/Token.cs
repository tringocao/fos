using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOS.API.Models
{
    public class Token
    {
        public string _accessToken { get; set; }
        public string _refreshToken { get; set; }
        public string _acessTokenExpireTime { get; set; }

        public Token(string accessToken, string refreshToken, string expireTime)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
            _acessTokenExpireTime = expireTime;
        }
    }
}