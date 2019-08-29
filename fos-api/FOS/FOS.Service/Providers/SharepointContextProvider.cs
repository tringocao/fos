using FOS.API.Models;
using FOS.Common;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.Providers
{
    public class SharepointContextProvider : ISharepointContextProvider
    {
        ITokenProvider _tokenProvider;
        public SharepointContextProvider(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public ClientContext GetSharepointContextFromUrl(string siteUrl)
        {
            Token token = _tokenProvider.GetTokenByResourceUrl(APIResource.SHAREPOINT_CONTEXT);
            var context = new ClientContext(siteUrl);
            context.AuthenticationMode = ClientAuthenticationMode.Default;
            context.FormDigestHandlingEnabled = false;
            context.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>((s, e) =>
            {
                e.WebRequestExecutor.WebRequest.Headers["Authorization"] = "Bearer " + token._accessToken;
                e.WebRequestExecutor.WebRequest.UserAgent = "FOS";
            });
            return context;
        }
    }
}
