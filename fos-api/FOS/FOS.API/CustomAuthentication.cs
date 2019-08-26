using FOS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace FOS.API
{
    public interface ICustomAuthentication
    {
        Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken);
        Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken);
    }

    public class CustomAuthentication : ICustomAuthentication, IAuthenticationFilter
    {
        public bool AllowMultiple => throw new NotImplementedException();

        IOAuthService _oAuthService;
        public CustomAuthentication(IOAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;

            var authenticated = _oAuthService.CheckAuthenticationAsync().Result;

            if (authenticated == false)
            {
                context.ErrorResult = new AuthenticationFailureResult(new { Error = 401, Message = "Unauthorized" }, request);
            }
        }
        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return;
        }
    }
}