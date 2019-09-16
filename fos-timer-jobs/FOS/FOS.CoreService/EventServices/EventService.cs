using FOS.CoreService.Constants;
using FOS.Services.SPUserService;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace FOS.CoreService.EventServices
{
    public class FosCoreService
    {
        public string BuildLink(string link, string text)
        {
            return "<a href=\"" + link + "\">" + text + "</a>";
        }
        public ListItemCollection GetListEventOpened(ClientContext clientContext)
        {
            var web = clientContext.Web;
            var list = web.Lists.GetByTitle(EventConstant.EventList);
            CamlQuery getAllEventOpened = new CamlQuery();
            getAllEventOpened.ViewXml =
                @"<View>
                        <Query>
                            <Where>
                                <Eq>" +
                                "<FieldRef Name=" + EventConstant.EventStatus + "/>" +
                                "<Value Type='Text'>" + EventStatus.Opened + "</Value>" +
                            @"</Eq>
                            </Where>
                        </Query>
                        <RowLimit>1000</RowLimit>
                    </View>";

            var events = list.GetItems(getAllEventOpened);
            clientContext.Load(events);
            clientContext.ExecuteQuery();

            return events;
        }
        public void CloseEvent(ClientContext clientContext, ListItem element)
        {
            element[EventConstant.EventStatus] = EventStatus.Closed;
            element.Update();
            clientContext.ExecuteQuery();
        }
        public Dictionary<string, string> GetEmailTemplate(string templateLink)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + templateLink;
            string emailTemplateJson = System.IO.File.ReadAllText(path);

            var emailTemplateDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(emailTemplateJson);

            return emailTemplateDictionary;
        }
        public void SendEmail(ClientContext clientContext, string fromMail, string toMail, string body, string subject)
        {
            var emailp = new EmailProperties();

            emailp.To = new List<string>() { toMail };
            emailp.From = fromMail;
            emailp.Body = body;
            emailp.Subject = subject;

            Utility.SendEmail(clientContext, emailp);
            clientContext.ExecuteQuery();
        }
        public ClientContext GetClientContext()
        {
            var siteUrl = "https://devpreciovn.sharepoint.com/sites/FOS/";
            var loginName = ConfigurationSettings.AppSettings["loginName"];
            var passWord = ConfigurationSettings.AppSettings["passWord"];
            var securePassword = new SecureString();
            passWord.ToCharArray().ToList().ForEach(c => securePassword.AppendChar(c));

            using (var clientContext = new ClientContext(siteUrl))
            {
                clientContext.Credentials = new SharePointOnlineCredentials(loginName, securePassword);
                return clientContext;
            }
        }
    }
}
