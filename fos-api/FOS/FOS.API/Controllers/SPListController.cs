using FOS.Common;
using FOS.Model.Domain;
using FOS.Services;
using FOS.Services.Providers;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using Microsoft.SharePoint.Client.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace FOS.API.Controllers
{
    public class SPListController : ApiController
    {
        IOAuthService _oAuthService;
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;

        public SPListController(IOAuthService oAuthService, IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider)
        {
            _oAuthService = oAuthService;
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
        }
        // GET api/splist/getlist/{list-id}
        public async Task<HttpResponseMessage> GetList(string Id)
        {
            return await _graphApiProvider.SendAsync(HttpMethod.Get, "sites/lists/" + Id, null);
        }
        // POST api/splist/addlistitem/{list-id}/
        public async Task<HttpResponseMessage> AddListItem(string Id, [FromBody]JSONRequest item)
        {
            return await _graphApiProvider.SendAsync(HttpMethod.Post, "sites/lists/" + Id + "/items/", item.data);
        }
        // POST api/splist/AddListItemCSOM/{list-id}/
        public async Task<HttpResponseMessage> AddListItemCSOM(string Id, [FromBody]EventList item)
        {
            var eventData = item;
            using (ClientContext context = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                Web web = context.Web;
                var loginName = "i:0#.f|membership|" + item.eventHost;
                //string email = eventData.eventHost;
                //PeopleManager peopleManager = new PeopleManager(context);
                //ClientResult<PrincipalInfo> principal = Utility.ResolvePrincipal(context, web, email, PrincipalType.User, PrincipalSource.All, web.SiteUsers, true);
                //context.ExecuteQuery();

                User newUser = context.Web.EnsureUser(loginName);
                context.Load(newUser);
                context.ExecuteQuery();

                FieldUserValue userValue = new FieldUserValue();
                userValue.LookupId = newUser.Id;

                List members = context.Web.Lists.GetByTitle("Event List");
                Microsoft.SharePoint.Client.ListItem listItem = members.AddItem(new ListItemCreationInformation());
                listItem["EventHost"] = userValue;
                listItem["EventTitle"] = eventData.eventTitle;
                listItem["EventId"] = 1;
                listItem["EventRestaurant"] = eventData.eventRestaurant;
                listItem["EventMaximumBudget"] = eventData.eventMaximumBudget;
                listItem["EventTimeToClose"] = eventData.eventTimeToClose;
                listItem["EventTimeToReminder"] = eventData.eventTimeToReminder;
                listItem["EventParticipants"] = eventData.eventParticipants;
                listItem.Update();
                context.ExecuteQuery();
            }

            HttpResponseMessage responde = Request.CreateResponse(HttpStatusCode.OK, "Create list item successfully");
            return responde;

        }
    }
}
