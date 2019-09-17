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

namespace FOS.RecurrenceEvent
{
    public partial class Service1 : ServiceBase
    {
        private string _threadOutput = "";
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
            timer.Interval = 20000; //number in milisecinds  

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

                foreach (var item in list)
                {
                    WriteToFile("Service is recall at " + item.Id);

                    var runningTask = Task.Factory.StartNew(() => DisplayThread1(item.Id));

                }
            }
            catch (Exception esss)
            {
                WriteToFile("Service is stopped at " + esss.ToString());

            }

        }
        void DisplayThread1(int a)
        {
            while (_stopThreads == false)
            {
                lock (this)
                {
                    WriteToFile("Display Thread " + a);

                    // Assign the shared memory to a message about thread #1  
                    _threadOutput = "Hello Thread " + a;


                    Thread.Sleep(1000);  // simulate a lot of processing   

                    // tell the user what thread we are in thread #1, and display shared memory  
                    WriteToFile("Thread " + a + " Output --> " + _threadOutput);
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
