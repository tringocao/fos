using FOS.Common;
using FOS.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FOS.API.Controllers
{
    public class HomeController : Controller
    {
        OAuthService _oAuthService = new OAuthService();

        //public HomeController()
        //{
        //    //_oAuthService = oAuthService;
        //}

        public async Task<ActionResult> Index()
        {
            //string url = Request.Url.AbsoluteUri;

            // ensure success responde before access code

            var authenticated = _oAuthService.CheckAuthentication().Result;

            if (authenticated == false)
            {
                var code = Request.QueryString["code"];

                if (code != null)
                {
                    var accessToken = await _oAuthService.GetToken(code);
                    return Redirect(OAuth.HOME_URI);
                }
                else
                {
                    return Redirect(_oAuthService.GetAuthCodePath()); // 401 
                }
            }
            return View();
        }
    }
}
