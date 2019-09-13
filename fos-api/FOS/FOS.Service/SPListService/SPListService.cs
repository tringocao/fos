﻿using FOS.Common;
using FOS.Model.Domain;
using FOS.Model.Util;
using FOS.Services.Providers;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.SPListService
{
    public class SPListService : ISPListService
    {
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;
        public SPListService(IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider)
        {
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
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
            catch (Exception)
            {
                return null;
            }
        }

        public async Task UpdateListItem(string id, Model.Domain.Event item)
        {
            try
            {
                var eventData = item;
                using (ClientContext context = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
                {
                    Web web = context.Web;
                    var loginName = item.HostName;

                    //var loginName = "i:0#.f|membership|" + item.eventHost;
                    //string email = eventData.eventHost;
                    //PeopleManager peopleManager = new PeopleManager(context);
                    //ClientResult<PrincipalInfo> principal = Utility.ResolvePrincipal(context, web, email, PrincipalType.User, PrincipalSource.All, web.SiteUsers, true);
                    //context.ExecuteQuery();

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

        public async Task UpdateEventParticipant(string id, string participants)
        {
            try
            {
                using (ClientContext context = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
                {
                    List members = context.Web.Lists.GetByTitle("Event List");

                    ListItem listItem = members.GetItemById(id);
                    listItem["EventParticipantsJson"] = participants;
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
