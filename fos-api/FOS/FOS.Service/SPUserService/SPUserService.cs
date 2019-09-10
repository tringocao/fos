using FOS.Common;
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

        public async Task<Model.Dto.User> GetCurrentUser()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "me", null);
            return await result.Content.ReadAsAsync<Model.Dto.User>();
        }

        public async Task<Model.Domain.User> GetUserById(string Id)
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "users/" + Id, null);
            return await result.Content.ReadAsAsync<Model.Domain.User>();
        }
        public async Task<string> GetGroups()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "groups", null);
            return await result.Content.ReadAsStringAsync();
        }
        public async Task<byte[]> GetAvatarByUserId(string Id)
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "users/" + Id + "/photos/48x48/$value", null);
            //return await result.Content.ReadAsStringAsync();
            var stream = await result.Content.ReadAsStreamAsync();
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            return bytes;
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
        public void AddEventListItem(string Id, EventListItem item)
        {
            var eventData = item;
            using (ClientContext context = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                Web web = context.Web;
                var loginName = item.EventHost;
                //var loginName = "i:0#.f|membership|" + item.eventHost;
                //string email = eventData.eventHost;
                //PeopleManager peopleManager = new PeopleManager(context);
                //ClientResult<PrincipalInfo> principal = Utility.ResolvePrincipal(context, web, email, PrincipalType.User, PrincipalSource.All, web.SiteUsers, true);
                //context.ExecuteQuery();

                Microsoft.SharePoint.Client.User newUser = context.Web.EnsureUser(loginName);
                context.Load(newUser);
                context.ExecuteQuery();

                FieldUserValue userValue = new FieldUserValue();
                userValue.LookupId = newUser.Id;

                List members = context.Web.Lists.GetByTitle("Event List");
                Microsoft.SharePoint.Client.ListItem listItem = members.AddItem(new ListItemCreationInformation());
                listItem["EventHost"] = userValue;
                listItem["EventTitle"] = eventData.EventTitle;
                listItem["EventId"] = 1;
                listItem["EventRestaurant"] = eventData.EventRestaurant;
                listItem["EventMaximumBudget"] = eventData.EventMaximumBudget;
                listItem["EventTimeToClose"] = eventData.EventTimeToClose;
                listItem["EventTimeToReminder"] = eventData.EventTimeToReminder;
                listItem["EventParticipants"] = eventData.EventParticipants;
                listItem["EventCategory"] = eventData.EventCategory;

                listItem["EventRestaurantId"] = eventData.EventRestaurantId;
                listItem["EventServiceId"] = eventData.EventServiceId;
                listItem["EventDeliveryId"] = eventData.EventDeliveryId;
                listItem["EventCreatedUserId"] = eventData.EventCreatedUserId;
                listItem["EventHostId"] = eventData.EventHostId;
                listItem.Update();
                context.ExecuteQuery();
            }
        }
    }
}
