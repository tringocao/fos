using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class ReportEmailTemplate
    {
        public string EventTitle { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string LinkToEventReport { get; set; }
    }
}
