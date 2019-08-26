using FOS.Common;
using FOS.Model.Domain;
using FOS.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace FOS.API.Controllers
{
    public class HomeController : Controller
    {
        OAuthService _oAuthService;

        public HomeController(OAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        public async Task<ActionResult> Index()
        {
            var authenticated = _oAuthService.CheckAuthenticationAsync().Result;

            if (authenticated == false)
            {
                return Redirect(_oAuthService.GetAuthCodePath(new State(
                    WebConfigurationManager.AppSettings[OAuth.HOME_URI]
                    )));
            }
            return View();
        }
    }
}
