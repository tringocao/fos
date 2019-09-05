using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOS.API.Models
{
    public class Token
    {
        public string _accessToken { get; set; }
        public string _acessTokenExpireTime { get; set; }
        public string _resource { get; set; }
    }
}