﻿using FOS.Common;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;
using FOS.Services.OrderServices;
using FOS.Services.Providers;
using FOS.Services.SPUserService;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;
using User = FOS.Model.Domain.User;
using FOS.Common.Constants;
using System.Web.Script.Serialization;

namespace FOS.Services.SendEmailServices
{
    public class SendEmailService : ISendEmailService
    {
        ISharepointContextProvider _sharepointContextProvider;
        ISPUserService _sPUserService;
        IOrderService _orderService;
        EmailTemplate emailTemplate;
        public SendEmailService(ISharepointContextProvider sharepointContextProvider, ISPUserService sPUserService, IOrderService orderService)
        {
            _sharepointContextProvider = sharepointContextProvider;
            _sPUserService = sPUserService;
            _orderService = orderService;
        }
        private async Task GetDataByEventIdAsync(ClientContext clientContext, string idEvent)
        {
            List list = clientContext.Web.Lists.GetByTitle("Event List");
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='ID'/>" +
                "<Value Type='Text'>" + idEvent + "</Value></Eq></Where></Query><RowLimit>1</RowLimit></View>";
            ListItemCollection collListItem = list.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();

            await SetValueForEmailAsync(collListItem.FirstOrDefault());
        }
        private async Task SetValueForEmailAsync(ListItem item)
        {
            var users = item["EventParticipantsJson"].ToString();
            emailTemplate.UsersEmail = JsonConvert.DeserializeObject<List<Model.Domain.User>>(users);
            string userId = item["EventHostId"].ToString();
            var user = await _sPUserService.GetUserByIdsDomain(userId);
            emailTemplate.HostUserEmail = user;
            emailTemplate.EventTitle = item["EventTitle"].ToString();
            emailTemplate.EventId = item["ID"].ToString();
            emailTemplate.EventRestaurant = item["EventRestaurant"].ToString();
            emailTemplate.EventRestaurantId = item["EventRestaurantId"].ToString();
            emailTemplate.EventDeliveryId = item["EventDeliveryId"].ToString();
        }
        public async Task SendEmailAsync(string idEvent, string html)
        {
            ReadEmailTemplate(html);
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                await GetDataByEventIdAsync(clientContext, idEvent);
                var emailp = new EmailProperties();
                string hostname = WebConfigurationManager.AppSettings[OAuth.HOME_URI];

                foreach (var user in emailTemplate.UsersEmail)
                {
                    Guid idOrder = Guid.NewGuid();
                    emailTemplate.MakeOrder = hostname + "make-order/" + idOrder;
                    emailp.To = new List<string>() { user.Mail };
                    emailp.From = emailTemplate.HostUserEmail.Mail;
                    emailp.BCC = new List<string> { emailTemplate.HostUserEmail.Mail };
                    emailp.Body = Parse(emailTemplate.Html.ToString(), user);
                    emailp.Subject = Parse(emailTemplate.Subject.ToString(), null);
                    
                    Utility.SendEmail(clientContext, emailp);
                    clientContext.ExecuteQuery();

                    _orderService.CreateOrderWithEmptyFoods(idOrder, user.Id, 
                        emailTemplate.EventRestaurantId, 
                        emailTemplate.EventDeliveryId, 
                        emailTemplate.EventId); ;
                }

            }
        }
        public async Task SendEmailToNotOrderedUserAsync(IEnumerable<UserNotOrderMailInfo> users, string emailTemplateJson)
        {
            var jsonTemplate = ReadEmailJsonTemplate(emailTemplateJson);
            var templateBody = jsonTemplate.TryGetValue("Body", out object body);
            ReadEmailTemplate(body.ToString());
            var templateSubject = jsonTemplate.TryGetValue("Subject", out object subject);
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                var emailp = new EmailProperties();
                string hostname = WebConfigurationManager.AppSettings[OAuth.HOME_URI];
                var host = await _sPUserService.GetCurrentUser();

                foreach (var user in users)
                {
                    emailp.To = new List<string>() { user.UserMail };
                    emailp.From = host.Mail;
                    emailp.BCC = new List<string> { host.Mail };
                    emailp.Body = String.Format(emailTemplate.Html.ToString(),
                        user.EventTitle,
                        user.EventRestaurant,
                        user.UserMail.ToString(),
                        hostname + "make-order/" + user.OrderId);
                    emailp.Subject = subject.ToString();

                    Utility.SendEmail(clientContext, emailp);
                    clientContext.ExecuteQuery();
                }
            }
        }
        private Dictionary<string, object> ReadEmailJsonTemplate(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
        public void ReadEmailTemplate(string html)
        {
            emailTemplate = JsonConvert.DeserializeObject<EmailTemplate>(html);

        }
        private string Parse(string text, User user)
        {
            var regex = new Regex(@"\[%Event.\S+%\]");
            var match = regex.Match(text);
            while (match.Success)
            {
                var value = match.Value;
                var memberName = ParseMemberName(value); //Some code you write to parse out the member name from the match value
                System.Reflection.PropertyInfo propertyInfo;
                object memberValue; 

                if (memberName != "Mail")
                {
                    propertyInfo = emailTemplate.GetType().GetProperty(memberName);
                    memberValue = propertyInfo.GetValue(emailTemplate, null);
                }
                else
                {
                    propertyInfo = user.GetType().GetProperty(memberName);
                    memberValue = propertyInfo.GetValue(user, null);
                }
                text = text.Replace(value, memberValue != null ? memberValue.ToString() : string.Empty);
                match = match.NextMatch();
            }
            return text;
        }
        private string ParseMemberName(string value)
        {
            return value.Split('.')[1].Split('%')[0];
        }
    }
}
