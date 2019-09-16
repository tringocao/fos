using FOS.CoreService.EventServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using Unity;
using FOS.CoreService.UnityConfig;
using FOS.CoreService.Constants;
using Microsoft.SharePoint.Client;

namespace FOS.ReminderService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer();
        FosCoreService coreService;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var container = new UnityContainer();
            RegisterUnity.Register(container);
            coreService = container.Resolve<FosCoreService>();

            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 6000; //number in milisecinds  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!System.IO.File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = System.IO.File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = System.IO.File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            //WriteToFile("Service is recall at " + DateTime.Now);

            //get event on sharepoint

        }

        //public async Task<int> GetEventToReminder(FosCoreService program)
        //{
        //    return await program.GetEventToReminder();
        //}

        private void GetListEventOpened()
        {
            using (var clientContext = coreService.GetClientContext())
            {
                var events = coreService.GetListEventOpened(clientContext);
                foreach (var element in events)
                {
                    var closeTimeString = element[EventConstant.EventTimeToClose].ToString();
                    var closeTime = DateTime.Parse(closeTimeString).ToLocalTime();


                }
            }
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
                                <And>
                                    <Eq>" +
                                        "<FieldRef Name=" + EventConstant.EventStatus + "/>" +
                                        "<Value Type='Text'>" + EventStatus.Opened + "</Value>" +
                                    @"</Eq> +
                                    <Lt>" +
                                        "<FieldRef Name=" + EventConstant.EventTimeToReminder + "/>" +
                                        "<Value Type='DateTime'  IncludeTimeValue='TRUE'>" + "10/16/2019 4:04:00 AM" + "</Value>" +
                                    @"</Lt> +
                               </And>
                            </Where>
                        </Query>
                        <RowLimit>1000</RowLimit>
                    </View>";

            var events = list.GetItems(getAllEventOpened);
            clientContext.Load(events);
            clientContext.ExecuteQuery();

            return events;
        }
    }
}
