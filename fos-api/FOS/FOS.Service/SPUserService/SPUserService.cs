using FOS.Model.Domain;
using FOS.Services.Providers;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.SPUserService
{
    public class SPUserService : ISPUserService
    {
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;

        public SPUserService(IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider)
        {
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
        }

        public async Task<string> GetUsers()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "users", null);
            var jsonString = await result.Content.ReadAsStringAsync();
            return jsonString;
        }

        public async Task<Model.Domain.User> GetCurrentUser()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "me", null);
            return await result.Content.ReadAsAsync<Model.Domain.User>();
        }

        public async Task<Model.Domain.User> GetUserById(string Id)
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "users/" + Id, null);
            return await result.Content.ReadAsAsync<Model.Domain.User>();
        }

        public async Task<string> GetAvatarById(string Id, string size)
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "/me/photo/$value", null);
            return await result.Content.ReadAsStringAsync();
        }

        //public async Task<HttpResponseMessage> GetContext()
        //{
        //    using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
        //    {
        //        var web = clientContext.Web;
        //        clientContext.Load(web);
        //        clientContext.ExecuteQuery();
        //        if (clientContext.Web.IsPropertyAvailable("Title"))
        //        {
        //            Console.WriteLine("Found title");
        //        }
        //        Console.WriteLine("Title: {0}", web.Title);
        //    }
        //    HttpResponseMessage responde = new HttpResponseMessage();

        //    return responde;
        //}
    }
}
