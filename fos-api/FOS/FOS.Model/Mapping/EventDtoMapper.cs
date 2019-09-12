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
            var closeDateString = ElementAttributeToString(element["EventTimeToClose"]);
            Nullable<DateTime> closeDate = SetDate(closeDateString);

            var remindDateString = ElementAttributeToString(element["EventTimeToReminder"]);
            Nullable<DateTime> remindDate = SetDate(remindDateString);

            var eventDateString = ElementAttributeToString(element["EventDate"]);
            Nullable<DateTime> eventDate = SetDate(eventDateString);

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
            eventModel.EventType = ElementAttributeToString(element["EventTypes"]);
            eventModel.EventDate = eventDate;
            eventModel.EventParticipantsJson = ElementAttributeToString(element["EventParticipantsJson"]);
            eventModel.DeliveryId = ElementAttributeToString(element["EventDeliveryId"]);
            eventModel.RemindTime = remindDate;

            return eventModel;
        }

        private string ElementAttributeToString(Object element)
        {
            return element != null ? element.ToString() : "";
        }
        private Nullable<DateTime> SetDate(string dateString)
        {
            var nullableDate = new Nullable<DateTime>();
            if(dateString.Length > 0)
            {
                nullableDate = DateTime.Parse(dateString).ToLocalTime();
            }
            else
            {
                nullableDate = null;
            }
            return nullableDate;
        }
    }
}
