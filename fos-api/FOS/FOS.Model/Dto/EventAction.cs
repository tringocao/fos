using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class EventAction
    {
        public bool CanViewEvent { get; set; }
        public bool CanEditEvent { get; set; }
        public bool CanCloneEvent { get; set; }
        public bool CanCloseEvent { get; set; }
        public bool CanMakeOrder { get; set; }
        public bool CanSendRemind { get; set; }
        public bool CanViewOrder { get; set; }
        public bool CanViewEventSummary { get; set; }
    }
}
