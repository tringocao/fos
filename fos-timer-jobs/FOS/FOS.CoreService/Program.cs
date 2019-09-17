using FOS.CoreService.Constants;
using FOS.CoreService.EventServices;
using FOS.CoreService.UnityConfig;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace FOS.CoreService
{
    public class Program
    {
        static EmailTemplate emailTemplate;
        public static void Main(string[] args)
        {
            var container = new UnityContainer();
            RegisterUnity.Register(container);
            FosCoreService coreService = container.Resolve<FosCoreService>();

            StartReminderService(coreService);
        }
        public static void sendMailToUserNotOrder(ClientContext clientContext, IEnumerable<UserNotOrderMailInfo> users, string emailTemplateJson)
        {
            try
            {
                var jsonTemplate = ReadEmailJsonTemplate(emailTemplateJson);
                var templateBody = jsonTemplate.TryGetValue("Body", out object body);
                ReadEmailTemplate(body.ToString());
                var templateSubject = jsonTemplate.TryGetValue("Subject", out object subject);
                var emailp = new EmailProperties();
                string hostname = "https://localhost:4200/";
                var noReplyEmail = ConfigurationSettings.AppSettings["noReplyEmail"];
                foreach (var user in users)
                {
                    emailp.To = new List<string>() { user.UserMail };
                    emailp.From = noReplyEmail;
                    emailp.Body = String.Format(emailTemplate.Html.ToString(),
                        user.EventTitle,
                        user.EventRestaurant,
                        user.UserMail.ToString(),
                        hostname + "make-order/" + user.OrderId);
                    emailp.Subject = subject.ToString();

                    Utility.SendEmail(clientContext, emailp);
                    //WriteFile.WriteToFile("User not order" + DateTime.Now);
                    clientContext.ExecuteQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static Dictionary<string, object> ReadEmailJsonTemplate(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }
        public static void ReadEmailTemplate(string html)
        {
            emailTemplate = JsonConvert.DeserializeObject<EmailTemplate>(html);
        }
        public async static void StartReminderService(FosCoreService coreService)
        {

            var timeToCheckMax = "2019-09-17T10:54:00";
            var timeToCheckMin = "2019-09-17T10:52:00";

            DateTime aDate = DateTime.Now;


            DateTime timeCheckMax = aDate.AddMinutes(1);
            DateTime timeCheckMin = aDate.AddMinutes(-1);

            var timeMax = timeCheckMax.ToString("yyyy-MM-ddTHH:mm:ss");
            var timeMin = timeCheckMin.ToString("yyyy-MM-ddTHH:mm:ss");

            //var dateToCheck = aDate.ToString("yyyy-MM-ddTHH:mm:ss");
            var clientContext = coreService.GetClientContext();
            var web = clientContext.Web;
            var list = web.Lists.GetByTitle(EventConstant.EventList);
            CamlQuery getAllEventOpened = new CamlQuery();
            getAllEventOpened.ViewXml =
                @"<View>
                        <Query>
                            <Where>
                                    <And>
                                        <And> +
                                        <Gt>" +
                                            "<FieldRef Name='" + EventConstant.EventTimeToReminder + "'/>" +
                                            "<Value Type='DateTime'  IncludeTimeValue='TRUE'>" + timeMin + "</Value>" +
                                        @"</Gt> +
                                        <Lt>" +
                                            "<FieldRef Name='" + EventConstant.EventTimeToReminder + "'/>" +
                                            "<Value Type='DateTime'  IncludeTimeValue='TRUE'>" + timeMax + "</Value>" +
                                        @"</Lt> +
                                    
                                        </And> +
                                        <And>
                                            <Eq>" +
                                                "<FieldRef Name='" + EventConstant.EventStatus + "'/>" +
                                                "<Value Type='Text'>" + EventStatus.Opened + "</Value>" +
                                            @"</Eq> +
                                            <Eq>" +
                                                "<FieldRef Name='" + EventConstant.EventIsReminder + "'/>" +
                                                "<Value Type='Text'>" + EventIsReminder.No + "</Value>" +
                                            @"</Eq> +
                                        </And>
                                     </And>
                            </Where>
                        </Query>
                        <RowLimit>1000</RowLimit>
                    </View>";

            var events = list.GetItems(getAllEventOpened);
            clientContext.Load(events);
            clientContext.ExecuteQuery();

            //WriteFile.WriteToFile("Number of event find: " + events.Count.ToString());

            //WriteFile.WriteToFile("--------------------------------------------");

            if (events.Count > 0)
            {
                List<Model.Dto.UserNotOrderMailInfo> lstUserNotOrder = new List<Model.Dto.UserNotOrderMailInfo>();

                foreach (var element in events)
                {
                    var eventId = element[EventConstant.ID].ToString();
                    UpdateEventIsReminder(clientContext, eventId, "Yes");
                    //WriteFile.WriteToFile("Update to remindered");

                    var eventTite = element[EventConstant.EventTitle].ToString();
                    var closeTimeString = element[EventConstant.EventTimeToClose].ToString();
                    var closeTime = DateTime.Parse(closeTimeString).ToLocalTime();
                    var eventRestaurant = element[EventConstant.EventRestaurant].ToString();
                    Console.WriteLine(eventTite);

                    var userNotOrder = coreService.GetEventToReminder(eventId);
                    //WriteFile.WriteToFile("Event find: " + eventTite.ToString());
                    foreach (var user in userNotOrder)
                    {
                        Model.Dto.UserNotOrderMailInfo userNew = new Model.Dto.UserNotOrderMailInfo();
                        userNew.EventRestaurant = eventRestaurant;
                        userNew.EventTitle = eventTite;
                        userNew.OrderId = user.OrderId;
                        userNew.UserMail = user.UserEmail;
                        lstUserNotOrder.Add(userNew);
                    }

                }
                //send mail

                string path = AppDomain.CurrentDomain.BaseDirectory + EventConstant.ReminderEventEmailTemplate;
                string emailTemplateJson = System.IO.File.ReadAllText(path);
                sendMailToUserNotOrder(clientContext, lstUserNotOrder, emailTemplateJson);
            }
        }
        public static void UpdateEventIsReminder(ClientContext context, string idEvent, string isReminder)
        {
            try
            {
                List members = context.Web.Lists.GetByTitle("Event List");

                ListItem listItem = members.GetItemById(idEvent);

                listItem["EventIsReminder"] = isReminder;
                listItem.Update();
                context.ExecuteQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
