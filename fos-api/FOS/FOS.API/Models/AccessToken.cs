using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOS.API.Models
{
    public class AccessToken
    {
        public string _key { get; set; }
        public string _expireTime { get; set; }

        public AccessToken(string key, string expireTime)
        {
            _key = key;
            _expireTime = expireTime;
        }
    }
}