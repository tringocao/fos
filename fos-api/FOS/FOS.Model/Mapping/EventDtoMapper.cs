using FOS.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace FOS.Model.Mapping
{
    public interface IEventDtoMapper
    {
        Event ListItemToEventModel(ListItem element);
    }
    public class EventDtoMapper : IEventDtoMapper
    {
        public Event ListItemToEventModel(ListItem element)
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

            var eventDateString = element["EventDate"].ToString();
            Nullable<DateTime> eventDate = DateTime.Parse(remindDateString).ToLocalTime();

            var eventModel = new Event();

            eventModel.Name = ElementAttributeToString(element["EventTitle"]);
            eventModel.Restaurant = ElementAttributeToString(element["EventRestaurant"]);
            eventModel.RestaurantId = ElementAttributeToString(element["EventRestaurantId"]);
            eventModel.Category = ElementAttributeToString(element["EventCategory"]);
            eventModel.CloseTime = closeDate;
            eventModel.Participants = ElementAttributeToString(element["EventParticipants"]);
            eventModel.MaximumBudget = ElementAttributeToString(element["EventMaximumBudget"]);
            eventModel.EventId = ElementAttributeToString(element["ID"]);
            eventModel.HostName = ElementAttributeToString(host.LookupValue);
            eventModel.HostId = ElementAttributeToString(element["EventHostId"]);
            eventModel.CreatedBy = ElementAttributeToString(element["EventCreatedUserId"]);
            eventModel.Status = ElementAttributeToString(element["EventStatus"]);
            eventModel.EventType = ElementAttributeToString(element["EventType"]);
            eventModel.EventDate = eventDate;
            eventModel.EventParticipantsJson = ElementAttributeToString(element["EventParticipantsJson"]);
            eventModel.RemindTime = remindDate;

            return eventModel;
        }

        private string ElementAttributeToString(Object element)
        {
            return element != null ? element.ToString() : "";
        }
    }
}
