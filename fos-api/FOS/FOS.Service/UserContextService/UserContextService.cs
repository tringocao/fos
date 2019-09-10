using FOS.Model.Domain;
using FOS.Services.Providers;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.UserContextService
{
    public class UserContextService : IUserContextService
    {
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;

        private Model.Domain.User user;

        public UserContextService(IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider)
        {
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
            //GetCurrentUserContextFromGraph();
        }

        private async void GetCurrentUserContextFromGraph(string url)
        {
            //var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "me", null);
            //this.user = await result.Content.ReadAsAsync<Model.Domain.User>();
            ClientContext context = _sharepointContextProvider.GetSharepointContextFromUrl(url);
            PeopleManager peopleManager = new PeopleManager(context);


        }

        public Model.Domain.User GetUserContext()
        {
            return user;
        } 
    }
}
