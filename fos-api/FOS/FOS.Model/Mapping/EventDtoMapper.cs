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
        Model.Domain.Event ToModel(Dto.Event eventDto);
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
            eventModel.DeliveryId = ElementAttributeToString(element["EventDeliveryId"]);
            eventModel.HostName = ElementAttributeToString(host.LookupValue);
            eventModel.HostId = ElementAttributeToString(element["EventHostId"]);
            eventModel.CreatedBy = ElementAttributeToString(element["EventCreatedUserId"]);
            eventModel.Status = ElementAttributeToString(element["EventStatus"]);
            eventModel.EventType = ElementAttributeToString(element["EventTypes"]);
            eventModel.EventDate = eventDate;
            eventModel.EventParticipantsJson = ElementAttributeToString(element["EventParticipantsJson"]);
            eventModel.RemindTime = remindDate;

            return eventModel;
        }

        public Model.Domain.Event ToModel(Dto.Event eventDto)
        {
            return new Domain.Event()
            {
                HostId = eventDto.HostId,
                HostName = eventDto.HostName,
                Action = null,
                Category = eventDto.Category,
                CloseTime = eventDto.CloseTime,
                RemindTime = eventDto.RemindTime,
                CreatedBy = eventDto.CreatedBy,
                DeliveryId = eventDto.DeliveryId,
                Restaurant = eventDto.Restaurant,
                RestaurantId = eventDto.RestaurantId,
                IsMyEvent = false,
                EventDate = eventDto.EventDate,
                EventId = eventDto.EventId,
                EventParticipantsJson = eventDto.EventParticipantsJson,
                EventType = eventDto.EventType,
                MaximumBudget = eventDto.MaximumBudget,
                Name = eventDto.Name,
                Participants = eventDto.Participants,
                ServiceId = eventDto.ServiceId,
                Status = eventDto.Status
            };
        }

        private string ElementAttributeToString(Object element)
        {
            return element != null ? element.ToString() : "";
        }
        private Nullable<DateTime> SetDate(string dateString)
        {
            var nullableDate = new Nullable<DateTime>();
            if (dateString.Length > 0)
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
