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
            var container = new UnityContainer();
            RegisterUnity.Register(container);
            coreService = container.Resolve<FosCoreService>();

            CloseEventAutomatically();

            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000; //number in milisecinds  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
        }
        private void CloseEventAutomatically()
        {
            using (var clientContext = coreService.GetClientContext())
            {
                var events = coreService.GetListEventOpened(clientContext);
                foreach (var element in events)
                {
                    var closeTimeString = element[EventConstant.EventTimeToClose].ToString();
                    var closeTime = DateTime.Parse(closeTimeString).ToLocalTime();

                    if (DateTime.Now >= closeTime)
                    {
                        ProcessAnEvent(clientContext, element);
                    }
                }
            }
        }
        private void ProcessAnEvent(ClientContext clientContext, ListItem element)
        {
            var clientUrl = ConfigurationSettings.AppSettings["clientUrl"];
            var noReplyEmail = ConfigurationSettings.AppSettings["noReplyEmail"];

            coreService.CloseEvent(clientContext, element);

            var emailTemplateDictionary = coreService.GetEmailTemplate(EventConstant.CloseEventEmailTemplate);
            emailTemplateDictionary.TryGetValue(EventEmail.Body, out string body);
            body = body.Replace(EventEmail.EventName, element[EventConstant.EventTitle].ToString())
                .Replace(EventEmail.EventSummaryLink, coreService.BuildLink(clientUrl + "/events/summary/" + element["ID"], "link"));
            emailTemplateDictionary.TryGetValue(EventEmail.Subject, out string subject);
            var host = element[EventConstant.EventHost] as FieldUserValue;

            coreService.SendEmail(clientContext, noReplyEmail, host.Email, body, subject);
        }
    }
}
