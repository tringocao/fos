using FOS.Common;
using FOS.Model.Domain;
using FOS.Model.Util;
using FOS.Services.Providers;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FOS.Services.OrderServices;

namespace FOS.Services.SPListService
{
    public class SPListService : ISPListService
    {
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;
        IOrderService _orderService;

        public SPListService(IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider, IOrderService orderService)
        {
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
            _orderService = orderService;
        }

        //public async Task<ApiResponse> GetList(string Id)
        //{
        //    var list = await _graphApiProvider.SendAsync(HttpMethod.Get, "sites/lists/" + Id, null);
        //    return ApiUtil.CreateSuccessfulResult();
        //}

        public async Task AddListItem(string id, JsonRequest item)
        {
            await _graphApiProvider.SendAsync(HttpMethod.Post, "sites/lists/" + id + "/items/", item.data);
        }

        public string AddEventListItem(string id, Model.Domain.Event item)
        {
            try
            {
                var eventData = item;
                using (ClientContext context = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
                {
                    Web web = context.Web;
                    var loginName = eventData.HostName;

                    Microsoft.SharePoint.Client.User newUser = context.Web.EnsureUser(loginName);
                    context.Load(newUser);
                    context.ExecuteQuery();

                    FieldUserValue userValue = new FieldUserValue();
                    userValue.LookupId = newUser.Id;

                    List members = context.Web.Lists.GetByTitle("Event List");
                    Microsoft.SharePoint.Client.ListItem listItem = members.AddItem(new ListItemCreationInformation());
                    listItem["EventHost"] = userValue;
                    listItem["EventTitle"] = eventData.Name;
                    listItem["EventId"] = 1;
                    listItem["EventRestaurant"] = eventData.Restaurant;
                    listItem["EventMaximumBudget"] = eventData.MaximumBudget;
                    listItem["EventTimeToClose"] = eventData.CloseTime;
                    listItem["EventTimeToReminder"] = eventData.RemindTime;
                    listItem["EventParticipants"] = eventData.Participants;
                    listItem["EventCategory"] = eventData.Category;

                    listItem["EventRestaurantId"] = eventData.RestaurantId;
                    listItem["EventServiceId"] = eventData.ServiceId;
                    listItem["EventDeliveryId"] = eventData.DeliveryId;
                    listItem["EventCreatedUserId"] = eventData.CreatedBy;
                    listItem["EventHostId"] = eventData.HostId;
                    listItem["EventParticipantsJson"] = eventData.EventParticipantsJson;
                    listItem["EventDate"] = eventData.EventDate;
                    listItem["EventStatus"] = eventData.Status;
                    listItem["EventTypes"] = eventData.EventType;
                    listItem.Update();
                    context.ExecuteQuery();

                    return listItem.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateListItem(string id, Model.Domain.Event item)
        {
            try
            {
                await DeleteOrderFromEvent(id);

                var eventData = item;
                using (ClientContext context = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
                {
                    Web web = context.Web;
                    var loginName = item.HostName;

                    Microsoft.SharePoint.Client.User newUser = context.Web.EnsureUser(loginName);
                    context.Load(newUser);
                    context.ExecuteQuery();

                    FieldUserValue userValue = new FieldUserValue();
                    userValue.LookupId = newUser.Id;

                    List members = context.Web.Lists.GetByTitle("Event List");

                    ListItem listItem = members.GetItemById(id);

                    listItem["EventHost"] = userValue;
                    listItem["EventTitle"] = eventData.Name;
                    listItem["EventId"] = 1;
                    listItem["EventRestaurant"] = eventData.Restaurant;
                    listItem["EventMaximumBudget"] = eventData.MaximumBudget;
                    listItem["EventTimeToClose"] = eventData.CloseTime;
                    listItem["EventTimeToReminder"] = eventData.RemindTime;
                    listItem["EventParticipants"] = eventData.Participants;
                    listItem["EventCategory"] = eventData.Category;

                    listItem["EventRestaurantId"] = eventData.RestaurantId;
                    listItem["EventServiceId"] = eventData.ServiceId;
                    listItem["EventDeliveryId"] = eventData.DeliveryId;
                    listItem["EventCreatedUserId"] = eventData.CreatedBy;
                    listItem["EventHostId"] = eventData.HostId;
                    listItem["EventParticipantsJson"] = eventData.EventParticipantsJson;
                    listItem["EventDate"] = eventData.EventDate;
                    listItem["EventStatus"] = eventData.Status;
                    listItem["EventTypes"] = eventData.EventType;
                    listItem.Update();
                    context.ExecuteQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task UpdateEventParticipant(string id, Model.Dto.GraphUser participant)
        {
            try
            {
                using (ClientContext context = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
                {
                    List members = context.Web.Lists.GetByTitle("Event List");

                    ListItem listItem = members.GetItemById(id);
                    context.Load(listItem, li => li["EventParticipantsJson"]);
                    context.ExecuteQuery();

                    var EventParticipantsJson = JsonConvert.DeserializeObject<List<Model.Dto.GraphUser>>(listItem.FieldValues["EventParticipantsJson"].ToString());
                    EventParticipantsJson.Add(participant);

                    listItem["EventParticipantsJson"] = JsonConvert.SerializeObject(EventParticipantsJson).ToString();
                    listItem.Update();
                    context.ExecuteQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Boolean> DeleteOrderFromEvent(string idEvent)
        {
            try
            {
                return  _orderService.DeleteOrderByIdEvent(idEvent);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task UpdateEventStatus(string id, string status)
        {
            try
            {
                using (ClientContext context = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
                {
                    List members = context.Web.Lists.GetByTitle("Event List");

                    ListItem listItem = members.GetItemById(id);
                    context.Load(listItem, li => li["EventStatus"]);
                    context.ExecuteQuery();

                    listItem["EventStatus"] = status;

                    listItem.Update();
                    context.ExecuteQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
