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
        UnityContainer container;
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

            container = new UnityContainer();
            RegisterUnity.Register(container);
            //try
            //{
            //    var remindEventServicce = container.Resolve<RemindEventService>();
            //    var coreService = container.Resolve<FosCoreService>();

            //    WriteToFile("Service is recall1 at " + DateTime.Now.ToLocalTime());
            //    var list = remindEventServicce.GetAllRecurranceEvents();
            //    DateTime today = DateTime.Today.ToLocalTime();
            //    foreach (var item in list)
            //    {
            //        WriteToFile("Service" + item.Id + " is recall5 at " + DateTime.Now.ToLocalTime() + " Id: " + item.Id + " status:" + item.IsReminding + " start:" + item.StartDate + " end:" + item.EndDate);
            //        if ((today >= item.StartDate.Date) && (today <= item.EndDate.Date) && !item.IsReminding)
            //        {
            //            if (item.StartDate.Hour == DateTime.Now.ToLocalTime().Hour)
            //            {
            //                WriteToFile("Service" + item.Id + " is recall6 at " + DateTime.Now.ToLocalTime() + " Id: " + item.Id + " status:" + item.IsReminding + " start:" + item.StartDate + " end:" + item.EndDate);

            //                switch (item.TypeRepeat)
            //                {
            //                    case RepeateType.Daily:
            //                        {
            //                            if (today == item.StartTempDate.Date)
            //                            {
            //                                item.IsReminding = true;
            //                                item.StartTempDate = item.StartTempDate.AddDays(1);
            //                            }
            //                            break;
            //                        }
            //                    case RepeateType.EveryWorkDay:
            //                        {
            //                            if (today == item.StartTempDate.Date)
            //                            {
            //                                item.StartTempDate = item.StartTempDate.AddDays(1);
            //                                if (today.DayOfWeek == DayOfWeek.Monday
            //                                || today.DayOfWeek == DayOfWeek.Tuesday
            //                                || today.DayOfWeek == DayOfWeek.Wednesday
            //                                || today.DayOfWeek == DayOfWeek.Thursday
            //                                || today.DayOfWeek == DayOfWeek.Friday
            //                                )
            //                                {
            //                                    item.IsReminding = true;
            //                                }

            //                            }
            //                            break;
            //                        }
            //                    case RepeateType.Monthly:
            //                        {
            //                            if (today == item.StartTempDate.Date)
            //                            {
            //                                item.IsReminding = true;
            //                                item.StartTempDate = item.StartTempDate.AddMonths(1);

            //                            }
            //                            break;
            //                        }
            //                    case RepeateType.Weekly:
            //                        {
            //                            if (today == item.StartTempDate.Date)
            //                            {
            //                                item.StartTempDate = item.StartTempDate.AddDays(7);
            //                                item.IsReminding = true;
            //                            }
            //                            break;
            //                        }
            //                    default:
            //                        {
            //                            if (today == item.StartTempDate.Date)
            //                            {
            //                                item.IsReminding = true;
            //                            }
            //                            break;
            //                        }

            //                }
            //                if (item.IsReminding)
            //                {
            //                    Task.Factory.StartNew(() => RunReminderAsync(item));

            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    WriteToFile("Service is stopped at " + ex.ToString());

            //}
            OnElapsedTime(null, null);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Enabled = true;
            timer.Interval = 60* 60 * 1000; //number in milisecinds  

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
                var remindEventServicce = container.Resolve<RemindEventService>();
                var coreService = container.Resolve<FosCoreService>();

               // WriteToFile("Service is recall1 at " + DateTime.Now.ToLocalTime());
                var list = remindEventServicce.GetAllRecurranceEvents();
                DateTime today = DateTime.Today.ToLocalTime();
                foreach (var item in list)
                {
                    //WriteToFile("Service" + item.Id + " is recall5 at " + DateTime.Now.ToLocalTime() + " Id: " + item.Id + " status:" + item.IsReminding + " start:" + item.StartDate + " end:" + item.EndDate);
                    if ((today >= item.StartDate.Date) && (today <= item.EndDate.Date) && !item.IsReminding)
                    {
                        if (item.StartDate.Hour == DateTime.Now.ToLocalTime().Hour)
                        {
                            //WriteToFile("Service" + item.Id + " is recall6 at " + DateTime.Now.ToLocalTime() + " Id: " + item.Id + " status:" + item.IsReminding + " start:" + item.StartDate + " end:" + item.EndDate);

                            switch (item.TypeRepeat)
                            {
                                case RepeateType.Daily:
                                    {
                                        if (today == item.StartTempDate.Date)
                                        {
                                            item.IsReminding = true;
                                            item.StartTempDate = item.StartTempDate.AddDays(1);
                                        }
                                        break;
                                    }
                                case RepeateType.EveryWorkDay:
                                    {
                                        if (today == item.StartTempDate.Date)
                                        {
                                            item.StartTempDate = item.StartTempDate.AddDays(1);
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
                                            item.StartTempDate = item.StartTempDate.AddMonths(1);

                                        }
                                        break;
                                    }
                                case RepeateType.Weekly:
                                    {
                                        if (today == item.StartTempDate.Date)
                                        {
                                            item.StartTempDate = item.StartTempDate.AddDays(7);
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
                            if (item.IsReminding)
                            {
                                Task.Factory.StartNew(() => RunReminderAsync(item));

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToFile("Service is stopped at " + ex.ToString());

            }

        }
        public void RunReminderAsync(Model.Domain.RecurrenceEvent startReEvent)
        {
            try
            {
                var remindEventServicce = container.Resolve<RemindEventService>();
                FosCoreService coreService;
                //WriteToFile("Service is sent1 email at " + startReEvent.Id + "IsReminding222: " + startReEvent.IsReminding);

                remindEventServicce.UpdateRecurrenceEvent(startReEvent);
                //WriteToFile("Service is sent1 email at " + startReEvent.Id + "IsReminding: " + startReEvent.IsReminding);
                while (startReEvent.IsReminding)
                {
                    remindEventServicce = container.Resolve<RemindEventService>();
                    coreService = container.Resolve<FosCoreService>();
                    WriteToFile("Service is sent1 email at " + DateTime.Now.ToLocalTime() + "Id: " + startReEvent.Id);
                    var item = remindEventServicce.GetRecurranceEventById(startReEvent.Id);
                    //WriteToFile("Service is sent1 email at " + item.UserMail + " Id: " + item.Id + " status:" + item.IsReminding + " start:" + item.StartDate + " end:" + item.EndDate);
                    DateTime today = DateTime.Today.ToLocalTime();
                    if ((today >= item.StartDate.Date)
                        && (today <= item.EndDate.Date)
                        && (item.StartDate.Hour == DateTime.Now.ToLocalTime().AddMilliseconds(500).Hour))
                    {
                        if (item.IsReminding == false)// false when update or sent email before
                        {
                            if ((startReEvent.StartDate.Minute - item.StartDate.Minute) >= 0)//when data update
                            {
                                item.IsReminding = true;
                                remindEventServicce.UpdateRecurrenceEvent(item);
                            }
                            else
                            {
                                startReEvent.IsReminding = false;
                                break;
                            }
                        }
                        if (DateTime.Now.ToLocalTime().Minute == item.StartDate.Minute)
                        {
                            //WriteToFile("Service is sent2 email at " + item.Id);
                            using (var clientContext = coreService.GetClientContext())
                            {
                                //var noReplyEmail = ConfigurationSettings.AppSettings["noReplyEmail"];
                                var emailTemplateDictionary = coreService.GetEmailTemplate(EventConstant.RemindEmailTemplate);
                                emailTemplateDictionary.TryGetValue(EventEmail.Body, out string body);
                                emailTemplateDictionary.TryGetValue(EventEmail.Subject, out string subject);
                                coreService.SendEmail(clientContext, "admin@devpreciovn.onmicrosoft.com", item.UserMail, remindEventServicce.Parse(body, item), subject);
                                WriteToFile("Service is sent email at! " + item.Id);
                                //Task.Delay(1000).Wait();
                                item.IsReminding = false;
                                remindEventServicce.UpdateRecurrenceEvent(item);
                            }
                        }
                        //Thread.Sleep(10000);
                        Task.Delay(60000).Wait();
                    }
                    else
                    {
                        item.IsReminding = false;
                        remindEventServicce.UpdateRecurrenceEvent(item);
                        WriteToFile("Service is sent email at!after " + item.Id);
                        startReEvent.IsReminding = false;
                    }
                }
                WriteToFile("Service is ended " + startReEvent.Id);
            }catch(Exception ee)
            {
                WriteToFile("Service is stopped at " + ee.ToString() + "Id: "+ startReEvent.Id);

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
