using FOS.Common;
using FOS.Services;
using FOS.Services.Providers;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace FOS.API.Controllers
{
    public class SPUserController : ApiController
    {
        IOAuthService _oAuthService;
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;

        public SPUserController(IOAuthService oAuthService, IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider)
        {
            _oAuthService = oAuthService;
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
        }

        // GET api/spuser/getusers
        public async Task<HttpResponseMessage> GetUsers()
        {
            return await _graphApiProvider.SendAsync(HttpMethod.Get, "users", null);
        }

        // GET api/spuser/GetCurrentUser
        public async Task<HttpResponseMessage> GetCurrentUser()
        {
            return await _graphApiProvider.SendAsync(HttpMethod.Get, "me", null);
        }

        // GET api/spuser/GetUserById/Id
        public async Task<HttpResponseMessage> GetUserById(string Id)
        {
            return await _graphApiProvider.SendAsync(HttpMethod.Get, "users/" + Id, null);
        }

        public async Task<HttpResponseMessage> GetContext()
        {
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                var web = clientContext.Web;
                clientContext.Load(web);
                clientContext.ExecuteQuery();
                if (clientContext.Web.IsPropertyAvailable("Title"))
                {
                    Console.WriteLine("Found title");
                }
                Console.WriteLine("Title: {0}", web.Title);
            }
            HttpResponseMessage responde = new HttpResponseMessage();

            return responde;

        }
        // GET api/spuser/getgroups
        public async Task<HttpResponseMessage> GetGroups()
        {
            return await _graphApiProvider.SendAsync(HttpMethod.Get, "groups", null);
        }
    }
}
