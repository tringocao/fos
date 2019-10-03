using FOS.Common;
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
using FOS.Model.Domain;
using FOS.Services.FosCoreService;

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
            List list = clientContext.Web.Lists.GetByTitle(EventFieldName.EventList);
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
            var users = item[EventFieldName.EventParticipantsJson].ToString();
            emailTemplate.UsersEmail = JsonConvert.DeserializeObject<List<Model.Domain.User>>(users);
            string userId = item[EventFieldName.EventHostId].ToString();
            var user = await _sPUserService.GetUserById(userId);
            emailTemplate.HostUserEmail = user;
            emailTemplate.EventTitle = item[EventFieldName.EventTitle].ToString();
            emailTemplate.EventId = item[EventFieldName.ID].ToString();
            emailTemplate.EventRestaurant = item[EventFieldName.EventRestaurant].ToString();
            emailTemplate.EventRestaurantId = item[EventFieldName.EventRestaurantId].ToString();
            emailTemplate.EventDeliveryId = item[EventFieldName.EventDeliveryId].ToString();
        }
        public async Task SendEmailAsync(string idEvent, string html)
        {
            try
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
                        emailTemplate.NotParticipant = hostname + "not-participant/" + idOrder;
                        emailp.To = new List<string>() { user.Mail };
                        emailp.From = emailTemplate.HostUserEmail.Mail;
                        //emailp.BCC = new List<string> { emailTemplate.HostUserEmail.Mail };
                        emailp.Body = Parse(Parse(emailTemplate.Html.ToString(), emailTemplate), user);
                        emailp.Subject = Parse(emailTemplate.Subject.ToString(), emailTemplate);

                        Utility.SendEmail(clientContext, emailp);
                        clientContext.ExecuteQuery();

                        await _orderService.CreateOrderWithEmptyFoods(idOrder, user.Id,
                             emailTemplate.EventRestaurantId,
                             emailTemplate.EventDeliveryId,
                             emailTemplate.EventId, user.Mail, EventEmail.NewOder);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task SendEmailToNotOrderedUserAsync(IEnumerable<UserNotOrderMailInfo> users, string emailTemplateJson)
        {
            var jsonTemplate = ReadEmailJsonTemplate(emailTemplateJson);
            jsonTemplate.TryGetValue("Body", out object body);
            ReadEmailTemplate(body.ToString());
            jsonTemplate.TryGetValue("Subject", out object subject);
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                var emailp = new EmailProperties();
                string hostname = WebConfigurationManager.AppSettings[OAuth.HOME_URI];
                var host = await _sPUserService.GetCurrentUser();

                foreach (var user in users)
                {
                    emailTemplate.MakeOrder = hostname + "make-order/" + user.OrderId;
                    emailTemplate.NotParticipant = hostname + "not-participant/" + user.OrderId;
                    emailp.To = new List<string>() { user.UserMail };
                    emailp.From = host.Mail;
                    emailp.Body = Parse(Parse(emailTemplate.Html.ToString(), emailTemplate), user);
                    emailp.Subject = Parse(subject.ToString(), user);

                    Utility.SendEmail(clientContext, emailp);
                    clientContext.ExecuteQuery();
                }
            }
        }
        public async Task SendEmailToReOrderEventAsync(List<Model.Domain.UserReorder> users, string emailTemplateJson)
        {
            var jsonTemplate = ReadEmailJsonTemplate(emailTemplateJson);
            jsonTemplate.TryGetValue("Body", out object body);
            ReadEmailTemplate(body.ToString());
            jsonTemplate.TryGetValue("Subject", out object subject);
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                try
                {
                    var emailp = new EmailProperties();
                    string hostname = WebConfigurationManager.AppSettings[OAuth.HOME_URI];
                    var host = await _sPUserService.GetCurrentUser();

                    foreach (var user in users)
                    {
                        user.FoodNameHtml = "";
                        foreach (var food in user.FoodName)
                        {
                            user.FoodNameHtml += "<li style=\"margin:10px\">" + food + "</li>";

                        }
                        emailTemplate.MakeOrder = hostname + "make-order/" + user.OrderId;
                        emailp.To = new List<string>() { user.UserMail };
                        emailp.From = host.Mail;
                        //emailp.BCC = new List<string> { host.Mail };
                        emailp.Body = Parse(Parse(emailTemplate.Html.ToString(), emailTemplate), user);
                        emailp.Subject = Parse(subject.ToString(), user);

                        Utility.SendEmail(clientContext, emailp);
                        clientContext.ExecuteQuery();
                    }
                }
                catch (Exception e)
                {
                    throw e;
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
        public string Parse<T>(string text, T modelparse)
        {
            var regex = new Regex(@"\[%" + modelparse.GetType().Name + @".\S+%\]");
            var match = regex.Match(text);
            while (match.Success)
            {
                var value = match.Value;
                var memberName = ParseMemberName(value);
                System.Reflection.PropertyInfo propertyInfo = modelparse.GetType().GetProperty(memberName);
                object memberValue = propertyInfo.GetValue(modelparse, null);
                text = text.Replace(value, memberValue != null ? memberValue.ToString() : string.Empty);
                match = match.NextMatch();
            }
            return text;
        }
        private string ParseMemberName(string value)
        {
            return value.Split('.')[1].Split('%')[0];
        }

        public async Task SendMailUpdateEvent(List<Model.Domain.GraphUser> removeListUser, List<Model.Domain.User> newListUser, string idEvent, string html)
        {
            foreach (var deleteUser in removeListUser)
            {
                _orderService.DeleteOrderByUserId(deleteUser.Id, idEvent);
            }

            ReadEmailTemplate(html);
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                await GetDataByEventIdAsync(clientContext, idEvent);
                var emailp = new EmailProperties();
                string hostname = WebConfigurationManager.AppSettings[OAuth.HOME_URI];

                foreach (var user in newListUser)
                {
                    Guid idOrder = Guid.NewGuid();
                    emailTemplate.MakeOrder = hostname + "make-order/" + idOrder;
                    emailp.To = new List<string>() { user.Mail };
                    emailp.From = emailTemplate.HostUserEmail.Mail;
                    //emailp.BCC = new List<string> { emailTemplate.HostUserEmail.Mail };
                    emailp.Body = Parse(Parse(emailTemplate.Html.ToString(), emailTemplate), user);
                    emailp.Subject = Parse(emailTemplate.Subject.ToString(), emailTemplate);

                    Utility.SendEmail(clientContext, emailp);
                    clientContext.ExecuteQuery();

                    _orderService.CreateOrderWithEmptyFoods(idOrder, user.Id,
                        emailTemplate.EventRestaurantId,
                        emailTemplate.EventDeliveryId,
                        emailTemplate.EventId, user.Mail, EventEmail.NewOder);
                }
            }
        }

        public async Task<IEnumerable<UserNotOrderMailInfo>> FilterUserIsParticipant(IEnumerable<UserNotOrderMailInfo> users)
        {
            try
            {
                List<UserNotOrderMailInfo> newList = new List<UserNotOrderMailInfo>();
                foreach (UserNotOrderMailInfo u in users.ToArray())
                {
                    var order = _orderService.GetOrder(new Guid(u.OrderId));
                    if (order.OrderStatus != 2)
                    {
                        newList.Add(u);
                    }
                }
                return newList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task SendEmailToAlreadyOrderedUserAsync(List<UserFeedbackMailInfo> users, string emailTemplateJson)
        {
            var jsonTemplate = ReadEmailJsonTemplate(emailTemplateJson);
            jsonTemplate.TryGetValue("Body", out object body);
            ReadEmailTemplate(body.ToString());
            jsonTemplate.TryGetValue("Subject", out object subject);
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                var emailp = new EmailProperties();
                string hostname = WebConfigurationManager.AppSettings[OAuth.HOME_URI];
                var host = await _sPUserService.GetCurrentUser();

                foreach (var user in users)
                {
                    emailTemplate.FeedBack = hostname + "/feedback/" + user.OrderId;
                    emailp.To = new List<string>() { user.UserMail };
                    emailp.From = host.Mail;
                    emailp.Body = Parse(Parse(emailTemplate.Html.ToString(), emailTemplate), user);
                    emailp.Subject = Parse(subject.ToString(), user);

                    Utility.SendEmail(clientContext, emailp);
                    clientContext.ExecuteQuery();
                }
            }
        }
        public async Task SendCancelEventMail(List<Model.Domain.EventUsers> listUser, Dictionary<string, string> emailTemplateDictionary)
        {


            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                var emailp = new EmailProperties();
                string hostname = WebConfigurationManager.AppSettings[OAuth.HOME_URI];
                var host = await _sPUserService.GetCurrentUser();
                List<Model.Domain.EventUsers> filterList = await FilterUser(listUser);
                foreach (var user in filterList)
                {
                    emailTemplateDictionary.TryGetValue("Body", out string body);
                    emailTemplateDictionary.TryGetValue("Subject", out string subject);

                    emailp.To = new List<string>() { user.UserMail };
                    emailp.From = host.Mail;
                    emailp.Body = Parse(body, user);
                    emailp.Subject = Parse(subject.ToString(), user);

                    Utility.SendEmail(clientContext, emailp);
                    clientContext.ExecuteQuery();
                }
            }
        }

        public Dictionary<string, string> GetEmailTemplate(string templateLink)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + templateLink;
            string emailTemplateJson = System.IO.File.ReadAllText(path);

            var emailTemplateDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(emailTemplateJson);
            return emailTemplateDictionary;
        }

        public async Task<List<Model.Domain.EventUsers>> FilterUser(List<Model.Domain.EventUsers> users)
        {
            try
            {
                List<Model.Domain.EventUsers> newList = new List<Model.Domain.EventUsers>();
                foreach (Model.Domain.EventUsers u in users)
                {
                    Model.Domain.Order order = _orderService.GetOrderByEventIdAndMail(u.EventId, u.UserMail).Result;
                    if (order.OrderStatus != EventEmail.NotOder)
                    {
                        newList.Add(u);
                    }
                }
                return newList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
