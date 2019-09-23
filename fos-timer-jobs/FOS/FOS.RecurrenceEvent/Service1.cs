
using System.ServiceProcess;

using Unity;
using System.Timers;
using System.Threading;
using System;
using FOS.Services.RecurrenceEventServices;
using System.IO;
using System.Threading.Tasks;
using FOS.Model.Domain;
using System.Configuration;
using Microsoft.SharePoint.Client.Utilities;
using FOS.RecurrenceEvent.UnityConfig;

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

            //WriteToFile("Service is started at " + DateTime.Now);

            container = new UnityContainer();
            RegisterUnity.Register(container);
            OnElapsedTime(null, null);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Enabled = true;
            timer.Interval = 60 * 60 * 1000; //number in milisecinds  

        }

        protected override void OnStop()
        {
            //WriteToFile("Service is stopped at " + DateTime.Now);
            Environment.Exit(0);


        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                var reminder = container.Resolve<RecurrenceEventService>();
                reminder.checkRemindedTask();

            }
            catch (Exception xe)
            {
                WriteToFile("Service is stopped at " + xe.ToString());
            }
        }
        public void WriteToFile(string Message)
        {
            try
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
            catch (Exception e)
            {
            }
        }
    }
}
