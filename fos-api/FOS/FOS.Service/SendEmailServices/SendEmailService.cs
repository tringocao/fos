using FOS.Common;
using FOS.Model.Dto;
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

namespace FOS.Services.SendEmailServices
{
    public class SendEmailService : ISendEmailService
    {
        ISharepointContextProvider _sharepointContextProvider;
        ISPUserService _sPUserService;
        EmailTemplate emailTemplate;
        public SendEmailService(ISharepointContextProvider sharepointContextProvider, ISPUserService sPUserService)
        {
            _sharepointContextProvider = sharepointContextProvider;
            _sPUserService = sPUserService;
        }
        private async Task GetDataByEventIdAsync(ClientContext clientContext, string idEvent)
        {
            List list = clientContext.Web.Lists.GetByTitle("Event List");
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='EventTitle'/>" +
                "<Value Type='Text'>" + idEvent + "</Value></Eq></Where></Query><RowLimit>1</RowLimit></View>";
            ListItemCollection collListItem = list.GetItems(camlQuery);
            clientContext.Load(collListItem);
            clientContext.ExecuteQuery();

            var users = Regex.Split(collListItem.FirstOrDefault()["EventParticipants"].ToString(), ";#");
            emailTemplate.UsersEmail = users.Select(u => new Model.Domain.User() { mail = u }).ToList();

            string userId = collListItem.FirstOrDefault()["EventHostId"].ToString();
            var user = await _sPUserService.GetUserById(userId);
            emailTemplate.HostUserEmail = user;

            emailTemplate.EventTitle = collListItem.FirstOrDefault()["EventTitle"].ToString();

            emailTemplate.Subject = "subject";

            emailTemplate.EventRestaurant = collListItem.FirstOrDefault()["EventRestaurant"].ToString();
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
                    emailp.To = new List<string>() { user.mail };
                    emailp.From = emailTemplate.HostUserEmail.mail;
                    emailp.BCC = new List<string> { emailTemplate.HostUserEmail.mail };
                    emailp.Body = String.Format(emailTemplate.Html.ToString(),
                        emailTemplate.EventTitle.ToString(),
                        emailTemplate.EventRestaurant.ToString(),
                        user.mail.ToString(),
                        hostname + "make-order/1");
                    emailp.Subject = emailTemplate.Subject;
                    Utility.SendEmail(clientContext, emailp);
                    clientContext.ExecuteQuery();
                }

            }
        }
        public void ReadEmailTemplate(string html)
        {
            emailTemplate = JsonConvert.DeserializeObject<EmailTemplate>(html);
        }
    }
}
