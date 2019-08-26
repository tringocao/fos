using FOS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace FOS.API
{
    public class CustomAuthentication : System.Web.Http.Filters.IAuthenticationFilter
    {
        public bool AllowMultiple => throw new NotImplementedException();

        OAuthService _oAuthService = new OAuthService();
        //public CustomAuthentication(OAuthService oAuthService)
        //{
        //    _oAuthService = oAuthService;
        //}

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;

            var authenticated = _oAuthService.CheckAuthentication().Result;

            if (authenticated == true)
            {
                context.ErrorResult = new AuthenticationFailureResult(new { Error = true, Message = "Token is invalid" }, request);
            }
            //return;
        }
        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return;
        }
    }
}