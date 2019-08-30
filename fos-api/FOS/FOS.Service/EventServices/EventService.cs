using FOS.Services.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.EventServices
{
    public class EventService : IEventService
    {
        public IEnumerable<EventModel> MapSharepointEventListToEventModel(ListItemCollection eventList)
        {
            var listEvent = new List<EventModel>();
            foreach(var element in eventList){
                var host = element["EventHost"] as FieldLookupValue;
                var dateString = element["EventTimeToClose"].ToString();
                Nullable<DateTime> date = DateTime.Parse(dateString).ToLocalTime();
                var eventModel = new EventModel();

                eventModel.Name = ElementAttributeToString(element["EventTitle"]);
                eventModel.Restaurant = ElementAttributeToString(element["EventRestaurant"]);
                eventModel.Category = ElementAttributeToString(element["EventCategory"]);
                eventModel.Date = date;
                eventModel.Participants = ElementAttributeToString(element["EventParticipants"]);
                eventModel.MaximumBudget = ElementAttributeToString(element["EventMaximumBudget"]);
                eventModel.EventId = ElementAttributeToString(element["EventId"]);
                eventModel.HostName = ElementAttributeToString(host.LookupValue);
                eventModel.HostId = ElementAttributeToString(element["EventHostId"]);
                eventModel.CreatedBy = ElementAttributeToString(element["EventCreatedUserId"]);
                
                listEvent.Add(eventModel);
            }
            return listEvent;
        }
        private string ElementAttributeToString(Object element)
        {
            return element != null ? element.ToString() : "";
        }
    }
}
