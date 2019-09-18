using FOS.CoreService.EventServices;
using FOS.CoreService.UnityConfig;
using System.ServiceProcess;

using Unity;
using System.Timers;
using System.Threading;
using System;
using FOS.CoreService.RemindEventServices;
using FOS.Services.RecurrenceEventServices;
using System.IO;
using System.Threading.Tasks;
using FOS.Model.Domain;
using System.Configuration;
using Microsoft.SharePoint.Client.Utilities;
using FOS.CoreService.Constants;

namespace FOS.RecurrenceEvent
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        RemindEventService remindEventServicce;
        FosCoreService coreService;
        public Service1()
        {
            InitializeComponent();
        }
        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {

            WriteToFile("Service is started at " + DateTime.Now);

            var container = new UnityContainer();
            RegisterUnity.Register(container);
            remindEventServicce = container.Resolve<RemindEventService>();
            coreService = container.Resolve<FosCoreService>();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Enabled = true;
            timer.Interval = 60 * 1000; //number in milisecinds  

        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
            Environment.Exit(0);


        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                WriteToFile("Service is recall1 at " + DateTime.Now.ToLocalTime());
                var list = remindEventServicce.GetAllRecurranceEvents();
                WriteToFile("Service is recall4 at " + DateTime.Now.ToLocalTime());

                DateTime today = DateTime.Today.ToLocalTime();
                WriteToFile("Service is recall3 at " + DateTime.Now.ToLocalTime());

                foreach (var item in list)
                {
                    WriteToFile("Service is recall5 at " + DateTime.Now.ToLocalTime());

                    if ((today >= item.StartDate.Date) && (today <= item.EndDate.Date) && !item.IsReminding)
                    {
                        if (item.StartDate.Hour == e.SignalTime.Hour)
                        {
                            switch (item.TypeRepeat)
                            {
                                case RepeateType.Daily:
                                    {
                                        if (today == item.StartTempDate.Date)
                                        {
                                            item.IsReminding = true;
                                            item.StartTempDate = item.StartDate.AddDays(1);
                                        }
                                        break;
                                    }
                                case RepeateType.EveryWorkDay:
                                    {
                                        if (today == item.StartTempDate.Date)
                                        {
                                            item.StartTempDate = item.StartDate.AddDays(1);
                                            if (today.DayOfWeek == DayOfWeek.Monday
                                            || today.DayOfWeek == DayOfWeek.Tuesday
                                            || today.DayOfWeek == DayOfWeek.Wednesday
                                            || today.DayOfWeek == DayOfWeek.Thursday
                                            || today.DayOfWeek == DayOfWeek.Friday
                                            )
                                            {
                                                item.IsReminding = true;
                                            }

                                        }
                                        break;
                                    }
                                case RepeateType.Monthly:
                                    {
                                        if (today == item.StartTempDate.Date)
                                        {
                                            item.IsReminding = true;
                                            item.StartTempDate = item.StartDate.AddMonths(1);

                                        }
                                        break;
                                    }
                                case RepeateType.Weekly:
                                    {
                                        if (today == item.StartTempDate.Date)
                                        {
                                            item.StartTempDate = item.StartDate.AddDays(7);
                                            item.IsReminding = true;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        if (today == item.StartTempDate.Date)
                                        {
                                            item.IsReminding = true;
                                        }
                                        break;
                                    }

                            }
                            WriteToFile("Service is recall2 at " + item.Id);
                            Task.Factory.StartNew(() => RunReminderAsync(e.SignalTime, item));

                            WriteToFile("Service is updated1 at " + item.IsReminding + " " + item.Id);

                            //remindEventServicce.UpdateRecurrenceEvent(item);

                            WriteToFile("Service is updated2 at " + item.IsReminding + " " + item.Id);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToFile("Service is stopped at " + ex.ToString());

            }

        }
        public void RunReminderAsync(DateTime runTime, Model.Domain.RecurrenceEvent recurrenceEvent)
        {
            while (recurrenceEvent.IsReminding)
            {
                WriteToFile("Service is sent1 email at " + DateTime.Now.ToLocalTime());

                DateTime today = DateTime.Today.ToLocalTime();
                var item = remindEventServicce.GetRecurranceEventById(recurrenceEvent.Id);               
                                                                                                            //check again condition, unless user change condition -> continue, if changing condition -> false -> kill task
                if ((today >= item.StartDate.Date)
                    && (today <= item.EndDate.Date)
                    && (item.StartDate.Hour == runTime.Hour))
                {
                    DateTime now = DateTime.Now.ToLocalTime();
                    if (now.Minute >= item.StartDate.Minute)
                    {
                        WriteToFile("Service is sent2 email at " + item.Id);

                        using (var clientContext = coreService.GetClientContext())
                        {

                            var noReplyEmail = ConfigurationSettings.AppSettings["noReplyEmail"];
                            var emailTemplateDictionary = coreService.GetEmailTemplate(EventConstant.RemindEmailTemplate);
                            emailTemplateDictionary.TryGetValue(EventEmail.Body, out string body);
                            emailTemplateDictionary.TryGetValue(EventEmail.Subject, out string subject);
                            coreService.SendEmail(clientContext, noReplyEmail, item.UserMail, remindEventServicce.Parse(body, item), subject);
                            WriteToFile("Service is sent email at! " + item.Id);
                            Task.Delay(1000).Wait();

                            remindEventServicce.UpdateRecurrenceEvent(item);
                            WriteToFile("Service is sent email at!after " + item.Id);

                        }
                        recurrenceEvent.IsReminding = false;
                        break;
                    }
                    if (item.IsReminding == false)
                    {
                        item.IsReminding = true;
                        remindEventServicce.UpdateRecurrenceEvent(item);
                    }
                    Task.Delay(60000).Wait();

                }
                else recurrenceEvent.IsReminding = false;
            }

        }
        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
