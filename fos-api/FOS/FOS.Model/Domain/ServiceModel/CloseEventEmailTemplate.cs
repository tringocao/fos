using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.ServiceModel
{
    public class CloseEventEmailTemplate
    {
        public string EventTitle { get; set; }
        public string EventSummaryLink { get; set; }
        public string HostName { get; set; }
    }
}
