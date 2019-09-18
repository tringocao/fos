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

namespace FOS.RecurrenceEvent
{
    public partial class Service1 : ServiceBase
    {
        private bool _stopThreads = false;
        System.Timers.Timer timer = new System.Timers.Timer();
        RemindEventServicce remindEventServicce;
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
            remindEventServicce = container.Resolve<RemindEventServicce>();


            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Enabled = true;
            timer.Interval = 24*60*60*1000; //number in milisecinds  

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
                WriteToFile("Service is recall at " + DateTime.Now);
                var list = remindEventServicce.GetAllRecurranceEvents();
                DateTime today = DateTime.Today;
                
                foreach (var item in list)
                {
                    if ((today >= item.StartDate.Date) && (today <= item.EndDate.Date) && !item.IsReminding)
                    {
                        switch (item.TypeRepeat)
                        {
                            case RepeateType.Daily: {
                                    item.IsReminding = true;//because WS repeat daily
                                    break;
                                }
                            case RepeateType.EveryWorkDay: {
                                    if(today.DayOfWeek == DayOfWeek.Monday 
                                        || today.DayOfWeek == DayOfWeek.Tuesday
                                        || today.DayOfWeek == DayOfWeek.Wednesday
                                        || today.DayOfWeek == DayOfWeek.Thursday
                                        || today.DayOfWeek == DayOfWeek.Friday
                                        )
                                    {
                                        item.IsReminding = true;
                                    }

                                    break; }
                            case RepeateType.Monthly: {
                                    if(today == item.StartTempDate.Date)
                                    {
                                        item.IsReminding = true;
                                        item.StartTempDate = DateTime.Now.AddMonths(1);

                                    }
                                    break; }
                            case RepeateType.Weekly: {
                                    if (today == item.StartTempDate.Date)
                                    {
                                        recurrenceEvent.StartTempDate = DateTime.Now.AddDays(7);
                                        item.IsReminding = true;
                                    }
                                    break; }

                        }
                        WriteToFile("Service is recall at " + item.Id);
                        Task.Factory.StartNew(() => RunReminderAsync(e.SignalTime, item));
                        remindEventServicce.UpdateRecurrenceEvent(item);
                    }
                }
            }
            catch (Exception esss)
            {
                WriteToFile("Service is stopped at " + esss.ToString());

            }

        }
        async Task RunReminderAsync(DateTime runTime, Model.Domain.RecurrenceEvent recurrenceEvent)
        {
            Model.Domain.User user = await remindEventServicce.GetUserByIdAsync(recurrenceEvent.UserId);// The await operator doesn't block the thread that evaluates the async method
            while (recurrenceEvent.IsReminding)
            {
                lock (this)// all task run in independence
                {
                    Task.Delay(1000);  // simulate a lot of processing   
                    //check again condition, unless user change condition -> continue, if changing condition -> 
                    sendEmail();
                }
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
