using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FOS.API.Models
{
    public class OAuthResponse
    {
        public int expires_in { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}