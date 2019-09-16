using FOS.CoreService.Constants;
using FOS.Services.SPUserService;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace FOS.CoreService.EventServices
{
    public class FosCoreService
    {
        ISPUserService _sPUserService;
        public FosCoreService(ISPUserService sPUserService)
        {
            _sPUserService = sPUserService;
        }
        public void ABC()
        {
            EventServicesAsync().Wait();
        }

        public async Task<int> EventServicesAsync()
        {
            var siteUrl = "https://devpreciovn.sharepoint.com/sites/FOS/";
            var clientUrl = ConfigurationSettings.AppSettings["clientUrl"];
            var loginName = ConfigurationSettings.AppSettings["loginName"];
            var passWord = ConfigurationSettings.AppSettings["passWord"];
            var securePassword = new SecureString();
            passWord.ToCharArray().ToList().ForEach(c => securePassword.AppendChar(c));

            using (var clientContext = new ClientContext(siteUrl))
            {
                clientContext.Credentials = new SharePointOnlineCredentials(loginName, securePassword);

                var web = clientContext.Web;
                var list = web.Lists.GetByTitle("Event List");
                clientContext.Load(list);
                clientContext.ExecuteQuery();

                CamlQuery getAllEventOpened = new CamlQuery();
                getAllEventOpened.ViewXml =
                    @"<View>
                        <Query>
                            <Where>
                                <Eq>
                                    <FieldRef Name='EventStatus'/>
                                    <Value Type='Text'>Opened</Value>
                                </Eq>
                            </Where>
                        </Query>
                        <RowLimit>1000</RowLimit>
                    </View>";

                var events = list.GetItems(getAllEventOpened);
                clientContext.Load(events);
                clientContext.ExecuteQuery();

                foreach (var element in events)
                {
                    var closeTimeString = element["EventTimeToClose"] != null ? element["EventTimeToClose"].ToString() : "";

                    var closeTime = DateTime.Parse(closeTimeString).ToLocalTime();

                    if (DateTime.Now >= closeTime)
                    {
                        element["EventStatus"] = "Closed";
                        element.Update();
                        clientContext.ExecuteQuery();

                        string path = AppDomain.CurrentDomain.BaseDirectory + EventConstant.CloseEventEmailTemplate;
                        string emailTemplateJson = System.IO.File.ReadAllText(path);

                        var emailTemplateDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(emailTemplateJson);
                        var templateBody = emailTemplateDictionary.TryGetValue("Body", out string body);

                        body.Replace("{EventName}", element["EventTitle"].ToString()).Replace("{EventSummaryLink}", clientUrl + "/events/summary/" + element["ID"]);
                        var templateSubject = emailTemplateDictionary.TryGetValue("Subject", out string subject);

                        var host = await _sPUserService.GetUserById(element["EventHostId"].ToString());

                        //var currentUser = await _sPUserService.GetCurrentUser();


                    }
                }
            }
            return 0;
        }
    }
}
