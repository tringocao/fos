using FOS.CoreService.UnityConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.ApplicationServices;
using Unity;

namespace FOS.RecurrenceEvent
{
    public partial class Service1 : ServiceBase
    {
        Thread Thread;

        readonly AutoResetEvent StopEvent;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var container = new UnityContainer();
            RegisterUnity.Register(container);
            RoleService = container.Resolve<FosCoreService>();

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
    }
}
