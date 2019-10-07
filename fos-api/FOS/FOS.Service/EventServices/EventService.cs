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
using FOS.Model.Domain;
using FOS.Model.Mapping;
using FOS.Common.Constants;

namespace FOS.Services.EventServices
{
    public class EventService : IEventService
    {
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;
        IEventDtoMapper _eventDtoMapper;
        public EventService(IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider, IEventDtoMapper eventDtoMapper)
        {
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
            _eventDtoMapper = eventDtoMapper;
        }
        public IEnumerable<Event> GetAllEvent(string userId)
        {
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "sites/FOS/"))
            {
                var web = clientContext.Web;
                var list = web.Lists.GetByTitle(EventFieldName.EventList);
                clientContext.Load(list);
                clientContext.ExecuteQuery();

                var collListItem = list.GetItems(CamlQuery.CreateAllItemsQuery());
                clientContext.Load(collListItem);
                clientContext.ExecuteQuery();

                var listEvent = new List<Event>();
                foreach (var element in collListItem)
                {
                    var eventModel = _eventDtoMapper.ListItemToEventModel(element);

                    IsMyEvent(eventModel, userId);
                    SetListAction(eventModel, userId);

                    listEvent.Add(eventModel);
                }
                return listEvent;
            }
        }
        private void IsMyEvent(Event eventModel, string userId)
        {
            var isParticipant = eventModel.EventParticipantsJson.Contains(userId);
            var isHost = eventModel.HostId == userId;

            eventModel.IsMyEvent = isParticipant || isHost || eventModel.CreatedBy == userId;
        }
        private void SetListAction(Event eventModel, string userId)
        {

            var isParticipant = eventModel.EventParticipantsJson.Contains(userId);
            var isHost = eventModel.HostId == userId;

            eventModel.Action = new EventAction
            {
                CanViewEvent = true,
                CanCloneEvent = true,
                CanEditEvent = isHost && (eventModel.Status == EventStatus.Opened || eventModel.Status == EventStatus.Reopened),
                CanCloseEvent = isHost && (eventModel.Status == EventStatus.Opened || eventModel.Status == EventStatus.Reopened),
                CanSendRemind = isHost && (eventModel.Status == EventStatus.Opened || eventModel.Status == EventStatus.Reopened),
                CanMakeOrder =
                            (isParticipant || eventModel.EventType == EventType.Open)
                            && (eventModel.Status == EventStatus.Opened || eventModel.Status == EventStatus.Reopened),
                CanViewOrder = eventModel.Status == EventStatus.Closed && isParticipant,
                CanViewEventSummary = isHost || isParticipant
            };
        }
        public Event GetEvent(int id)
        {
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "sites/FOS/"))
            {
                var web = clientContext.Web;
                var list = web.Lists.GetByTitle(EventFieldName.EventList);
                clientContext.Load(list);
                clientContext.ExecuteQuery();

                var item = list.GetItemById(id);
                clientContext.Load(item);
                clientContext.ExecuteQuery();

                var result = _eventDtoMapper.ListItemToEventModel(item);

                return result;
            }
        }
        public bool ValidateEventInfo(Event eventInfo)
        {
            try
            {
                bool valid = true;
                if (eventInfo.Name == "")
                {
                    valid = false;
                }
                bool isNumeric = int.TryParse(eventInfo.MaximumBudget, out int n);
                if (isNumeric == false)
                {
                    valid = false;
                }
                DateTime temp;
                if (DateTime.TryParse(eventInfo.EventDate.ToString(), out temp) == false)
                {
                    valid = false;
                }
                if (DateTime.TryParse(eventInfo.CloseTime.ToString(), out temp) == false)
                {
                    valid = false;
                }
                if(eventInfo.RemindTime != null)
                {
                    if (DateTime.TryParse(eventInfo.RemindTime.ToString(), out temp) == false)
                    {
                        valid = false;
                    }
                }
                if(eventInfo.RestaurantId == null)
                {
                    valid = false;
                }
                if(eventInfo.HostName == null)
                {
                    valid = false;
                }
                if(eventInfo.EventParticipantsJson == null)
                {
                    valid = false;
                }
                if(Int32.Parse(eventInfo.Participants) == 0)
                {
                    valid = false;
                }
                return valid;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}