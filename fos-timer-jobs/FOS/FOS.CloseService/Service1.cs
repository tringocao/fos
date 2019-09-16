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
using FOS.CoreService;
using Unity;
using FOS.CoreService.UnityConfig;
using FOS.CoreService.EventServices;
using FOS.CoreService.Constants;
using Microsoft.SharePoint.Client;
using System.Configuration;
using FOS.CoreService.Models;

namespace FOS.CloseService
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
            CloseEvent();

            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000; //number in milisecinds  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            CloseEvent();
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
        private void CloseEvent()
        {
            //WriteToFile("Close event begin at: " + DateTime.Now);
            var container = new UnityContainer();
            RegisterUnity.Register(container);
            coreService = container.Resolve<FosCoreService>();

            using (var clientContext = coreService.GetClientContext())
            {
                var events = coreService.GetListEventShouldClose(clientContext);
                foreach (var element in events)
                {
                    CloseAnEvent(clientContext, element);
                }
            }
            //WriteToFile("Close event done at: " + DateTime.Now);
        }
        private void CloseAnEvent(ClientContext clientContext, ListItem element)
        {
            var clientUrl = ConfigurationSettings.AppSettings["clientUrl"];
            var noReplyEmail = ConfigurationSettings.AppSettings["noReplyEmail"];

            coreService.ChangeStatusToClose(clientContext, element);

            CloseEventEmailTemplate emailTemplate = new CloseEventEmailTemplate();
            emailTemplate.EventTitle = element[EventConstant.EventTitle].ToString();
            emailTemplate.EventSummaryLink = coreService.BuildLink(clientUrl + "/events/summary/" + element["ID"], "link");
            var emailTemplateDictionary = coreService.GetEmailTemplate(EventConstant.CloseEventEmailTemplate);
            emailTemplateDictionary.TryGetValue(EventEmail.Body, out string body);
            body = coreService.Parse(body, emailTemplate);
            emailTemplateDictionary.TryGetValue(EventEmail.Subject, out string subject);
            var host = element[EventConstant.EventHost] as FieldUserValue;

            coreService.SendEmail(clientContext, noReplyEmail, host.Email, body, subject);
        }
    }
}
