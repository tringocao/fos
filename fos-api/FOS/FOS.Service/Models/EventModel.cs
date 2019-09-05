using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.Models
{
    public class EventModel
    {
        public string Name { get; set; }
        public string Restaurant { get; set; }
        public string Category { get; set; }
        public string Participants { get; set; }
        public string MaximumBudget { get; set; }
        public string HostName { get; set; }
        public string HostId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Date { get; set; }
        public string EventId { get; set; }
        public string Status { get; set; }
        public DateTime? TimeToRemind { get; set; }
    }
}
