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
using FOS.CoreService.EventServices;
using Unity;
using FOS.CoreService.UnityConfig;

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
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000; //number in milisecinds  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service is recall at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);
        }
        public void WriteToFile(string Message)
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            //string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            //if (!File.Exists(filepath))
            //{
            //    // Create a file to write to.   
            //    using (StreamWriter sw = File.CreateText(filepath))
            //    {
            //        sw.WriteLine(Message);
            //    }
            //}
            //else
            //{
            //    using (StreamWriter sw = File.AppendText(filepath))
            //    {
            //        sw.WriteLine(Message);
            //    }
            //}
            GetData(coreService).Wait();
        }
        public async Task<int> GetData(FosCoreService program)
        {
            return await program.EventServicesAsync();
        }
    }
}
