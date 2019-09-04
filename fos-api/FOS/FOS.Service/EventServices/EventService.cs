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
        public IEnumerable<EventModel> GetAllEvent()
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

                var listEvent = new List<EventModel>();
                foreach (var element in collListItem)
                {
                    var host = element["EventHost"] as FieldLookupValue;
                    if(host == null)
                    {
                        host = new FieldLookupValue();
                    }
                    var dateString = element["EventTimeToClose"].ToString();
                    Nullable<DateTime> date = DateTime.Parse(dateString).ToLocalTime();
                    var eventModel = new EventModel();

                    eventModel.Name = ElementAttributeToString(element["EventTitle"]);
                    eventModel.Restaurant = ElementAttributeToString(element["EventRestaurant"]);
                    eventModel.Category = ElementAttributeToString(element["EventCategory"]);
                    eventModel.Date = date;
                    eventModel.Participants = ElementAttributeToString(element["EventParticipants"]);
                    eventModel.MaximumBudget = ElementAttributeToString(element["EventMaximumBudget"]);
                    eventModel.EventId = ElementAttributeToString(element["ID"]);
                    eventModel.HostName = ElementAttributeToString(host.LookupValue);
                    eventModel.HostId = ElementAttributeToString(element["EventHostId"]);
                    eventModel.CreatedBy = ElementAttributeToString(element["EventCreatedUserId"]);
                    eventModel.Status = ElementAttributeToString(element["status"]);

                    listEvent.Add(eventModel);
                }
                return listEvent;
            }
        }
        private string ElementAttributeToString(Object element)
        {
            return element != null ? element.ToString() : "";
        }
    }
}
