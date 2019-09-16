using FOS.CoreService.Constants;
using FOS.CoreService.EventServices;
using FOS.CoreService.UnityConfig;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace FOS.CoreService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new UnityContainer();
            RegisterUnity.Register(container);
            FosCoreService coreService = container.Resolve<FosCoreService>();
            //GetListEventOpened(coreService);

            TestGetEventWithCurrent(coreService);
            //SendMailRemider(coreService);


        }

        private static void GetListEventOpened(FosCoreService coreService)
        {
            using (var clientContext = coreService.GetClientContext())
            {
                var events = coreService.GetListEventOpened(clientContext);
                foreach (var element in events)
                {
                    var eventTite = element[EventConstant.EventTitle].ToString();
                    var closeTimeString = element[EventConstant.EventTimeToClose].ToString();
                    var closeTime = DateTime.Parse(closeTimeString).ToLocalTime();
                    Console.WriteLine(eventTite);
                }
            }


        }
        public static void TestGetEventWithCurrent(FosCoreService coreService)
        {
            var timeToCheckMax = "2019-09-16T08:02:00";
            var timeToCheckMin = "2019-09-16T08:00:00";

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
                                    <And> +
                                    <Gt>" +
                                        "<FieldRef Name='" + EventConstant.EventTimeToReminder + "'/>" +
                                        "<Value Type='DateTime'  IncludeTimeValue='TRUE'>" + timeToCheckMin + "</Value>" +
                                    @"</Gt> +
                                      <Lt>" +
                                        "<FieldRef Name='" + EventConstant.EventTimeToReminder + "'/>" +
                                        "<Value Type='DateTime'  IncludeTimeValue='TRUE'>" + timeToCheckMax + "</Value>" +
                                    @"</Lt> +
                                    </And>
                            </Where>
                        </Query>
                        <RowLimit>1000</RowLimit>
                    </View>";

            var events = list.GetItems(getAllEventOpened);
            clientContext.Load(events);
            clientContext.ExecuteQuery();

            foreach (var element in events)
            {
                var eventTite = element[EventConstant.EventTitle].ToString();
                var closeTimeString = element[EventConstant.EventTimeToClose].ToString();
                var closeTime = DateTime.Parse(closeTimeString).ToLocalTime();
                Console.WriteLine(eventTite);
                var eventId = element[EventConstant.ID].ToString();
                IEnumerable<Model.Dto.UserNotOrder> userNotOrder = coreService.GetEventToReminder(eventId);
            }

        }
        public static void SendMailRemider(FosCoreService coreService)
        {
            coreService.SendMailRemider(null);
        }

        public static string GetMailByUserId(FosCoreService coreService, string userId)
        {
            var clientContext = coreService.GetClientContext();

            return "";
        }
    }
}
