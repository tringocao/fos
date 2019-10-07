using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using FOS.Common.Constants;

namespace FOS.Model.Mapping
{
    public interface IEventDtoMapper
    {
        Event ListItemToEventModel(ListItem element);
        Event DtoToDomain(Model.Dto.Event eventDto);
        Dto.Event DomainToDto(Domain.Event eventDomain);
        IEnumerable<Dto.Event> ListDomainToDto(IEnumerable<Domain.Event> eventDomain);
    }
    public class EventDtoMapper : IEventDtoMapper
    {
        public Event ListItemToEventModel(ListItem element)
        {
            var host = element[EventFieldName.EventHost] as FieldLookupValue;
            if (host == null)
            {
                host = new FieldLookupValue();
            }
            var closeDateString = ElementAttributeToString(element[EventFieldName.EventTimeToClose]);
            Nullable<DateTime> closeDate = SetDate(closeDateString);

            var remindDateString = ElementAttributeToString(element[EventFieldName.EventTimeToReminder]);
            Nullable<DateTime> remindDate = SetDate(remindDateString);

            var eventDateString = ElementAttributeToString(element[EventFieldName.EventDate]);
            Nullable<DateTime> eventDate = SetDate(eventDateString);

            var eventModel = new Event();

            eventModel.Name = ElementAttributeToString(element[EventFieldName.EventTitle]);
            eventModel.Restaurant = ElementAttributeToString(element[EventFieldName.EventRestaurant]);
            eventModel.RestaurantId = ElementAttributeToString(element[EventFieldName.EventRestaurantId]);
            eventModel.Category = ElementAttributeToString(element[EventFieldName.EventCategory]);
            eventModel.CloseTime = closeDate;
            eventModel.Participants = ElementAttributeToString(element[EventFieldName.EventParticipants]);
            eventModel.MaximumBudget = ElementAttributeToString(element[EventFieldName.EventMaximumBudget]);
            eventModel.EventId = ElementAttributeToString(element[EventFieldName.ID]);
            eventModel.DeliveryId = ElementAttributeToString(element[EventFieldName.EventDeliveryId]);
            eventModel.HostName = ElementAttributeToString(host.LookupValue);
            eventModel.HostId = ElementAttributeToString(element[EventFieldName.EventHostId]);
            eventModel.CreatedBy = ElementAttributeToString(element[EventFieldName.EventCreatedUserId]);
            eventModel.Status = ElementAttributeToString(element[EventFieldName.EventStatus]);
            eventModel.EventType = ElementAttributeToString(element[EventFieldName.EventTypes]);
            eventModel.EventDate = eventDate;
            eventModel.EventParticipantsJson = ElementAttributeToString(element[EventFieldName.EventParticipantsJson]);
            eventModel.ServiceId = ElementAttributeToString(element[EventFieldName.EventServiceId]);

            eventModel.RemindTime = remindDate;

            return eventModel;
        }
        public Event DtoToDomain(Model.Dto.Event eventDto)
        {
            return new Event()
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
        public Dto.Event DomainToDto(Domain.Event eventDomain)
        {
            Dto.EventAction domainEventAction = null;

            if (eventDomain.Action != null)
            {
                domainEventAction = new Dto.EventAction()
                {
                    CanCloseEvent = eventDomain.Action.CanCloseEvent,
                    CanEditEvent = eventDomain.Action.CanEditEvent,
                    CanCloneEvent = eventDomain.Action.CanCloneEvent,
                    CanMakeOrder = eventDomain.Action.CanMakeOrder,
                    CanSendRemind = eventDomain.Action.CanSendRemind,
                    CanViewEvent = eventDomain.Action.CanViewEvent,
                    CanViewOrder = eventDomain.Action.CanViewOrder,
                    CanViewEventSummary = eventDomain.Action.CanViewEventSummary
                };
            }
            
            return new Dto.Event()
            {
                HostId = eventDomain.HostId,
                HostName = eventDomain.HostName,
                Action = domainEventAction,
                Category = eventDomain.Category,
                CloseTime = eventDomain.CloseTime,
                RemindTime = eventDomain.RemindTime,
                CreatedBy = eventDomain.CreatedBy,
                DeliveryId = eventDomain.DeliveryId,
                Restaurant = eventDomain.Restaurant,
                RestaurantId = eventDomain.RestaurantId,
                IsMyEvent = eventDomain.IsMyEvent,
                EventDate = eventDomain.EventDate,
                EventId = eventDomain.EventId,
                EventParticipantsJson = eventDomain.EventParticipantsJson,
                EventType = eventDomain.EventType,
                MaximumBudget = eventDomain.MaximumBudget,
                Name = eventDomain.Name,
                Participants = eventDomain.Participants,
                ServiceId = eventDomain.ServiceId,
                Status = eventDomain.Status
            };
        }
        public IEnumerable<Dto.Event> ListDomainToDto (IEnumerable<Domain.Event> eventDomain)
        {
            return eventDomain.Select(c => DomainToDto(c)).ToList();
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
