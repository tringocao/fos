using FOS.Services.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Services.Providers;
using FOS.Model.Util;
using FOS.Common;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Dynamic;

namespace FOS.Services.EventServices
{
    public class EventService : IEventService
    {
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;
        public EventService(IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider)
        {
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
        }
        public async Task<IEnumerable<EventModel>> GetAllEvent(string userId)
        {
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "sites/FOS/"))
            {
                var web = clientContext.Web;
                var list = web.Lists.GetByTitle("Event List");
                clientContext.Load(list);
                clientContext.ExecuteQuery();

                var collListItem = list.GetItems(CamlQuery.CreateAllItemsQuery());
                clientContext.Load(collListItem);
                clientContext.ExecuteQuery();

                //char[] seperator = { ';', '#' };

                //var groups = await _graphApiProvider.SendAsync(HttpMethod.Get, "groups", null);
                //var data = groups.Content.ReadAsStringAsync();
                //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                //dynamic groupList = jsonSerializer.Deserialize<dynamic>(data.Result);

                var listEvent = new List<EventModel>();
                foreach (var element in collListItem)
                {
                    //var participant = ElementAttributeToString(element["EventParticipants"]).Split(seperator).ToList();
                    //participant.RemoveAll(item => item == "");

                    //var isBelongMyEvent = false;
                    //foreach (var item in participant)
                    //{
                    //    isBelongMyEvent  = await IsUserBelongGroupParticipant(item, groupList);
                    //    if (isBelongMyEvent)
                    //    {
                    //        break;
                    //    }
                    //}
                    var eventModel = ListItemToEventModel(element, userId);
                    listEvent.Add(eventModel);
                }
                return listEvent;
            }
        }
        private async Task<Boolean> IsUserBelongGroupParticipant(string mail, dynamic groupList)
        {

            foreach (var item in groupList["value"])
            {
                if(Equals(item["mail"], mail))
                {
                    var groupId = "\'" + item["id"] + "\'";
                    var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "me/memberOf?$filter=id eq " + groupId, null);
                    var data = result.Content.ReadAsStringAsync();

                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    dynamic group = jsonSerializer.Deserialize<dynamic>(data.Result);

                    if(group["value"][0] != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private string ElementAttributeToString(Object element)
        {
            return element != null ? element.ToString() : "";
        }

        private EventModel ListItemToEventModel(ListItem element, string userId)
        {
            var host = element["EventHost"] as FieldLookupValue;
            if (host == null)
            {
                host = new FieldLookupValue();
            }
            var closeDateString = element["EventTimeToClose"].ToString();
            Nullable<DateTime> closeDate = DateTime.Parse(closeDateString).ToLocalTime();

            var remindDateString = element["EventTimeToReminder"].ToString();
            Nullable<DateTime> remindDate = DateTime.Parse(remindDateString).ToLocalTime();

            var eventModel = new EventModel();

            eventModel.Name = ElementAttributeToString(element["EventTitle"]);
            eventModel.Restaurant = ElementAttributeToString(element["EventRestaurant"]);
            eventModel.Category = ElementAttributeToString(element["EventCategory"]);
            eventModel.CloseTime = closeDate;
            eventModel.Participants = ElementAttributeToString(element["EventParticipants"]);
            eventModel.MaximumBudget = ElementAttributeToString(element["EventMaximumBudget"]);
            eventModel.EventId = ElementAttributeToString(element["ID"]);
            eventModel.HostName = ElementAttributeToString(host.LookupValue);
            eventModel.HostId = ElementAttributeToString(element["EventHostId"]);
            eventModel.CreatedBy = ElementAttributeToString(element["EventCreatedUserId"]);
            eventModel.Status = ElementAttributeToString(element["status"]);
            eventModel.RemindTime = remindDate;

            eventModel.IsMyEvent = 
                eventModel.Participants.Contains(userId) 
                || eventModel.HostId == userId 
                || eventModel.CreatedBy == userId;

            return eventModel;
        }

        public EventModel GetEvent(int id)
        {
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "sites/FOS/"))
            {
                var web = clientContext.Web;
                var list = web.Lists.GetByTitle("Event List");
                clientContext.Load(list);
                clientContext.ExecuteQuery();

                var item = list.GetItemById(id);
                clientContext.Load(item);
                clientContext.ExecuteQuery();

                var result = ListItemToEventModel(item, null);

                return result;
            }
        }
    }
}