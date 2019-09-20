using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class Event
    {
        public string Name { get; set; }
        public string Restaurant { get; set; }
        public string RestaurantId { get; set; }
        public string Category { get; set; }
        public string Participants { get; set; }
        public string MaximumBudget { get; set; }
        public string DeliveryId { get; set; }
        public string HostName { get; set; }
        public string HostId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CloseTime { get; set; }
        public string EventId { get; set; }
        public string Status { get; set; }
        public DateTime? RemindTime { get; set; }
        public Boolean IsMyEvent { get; set; }
        public string EventType { get; set; }
        public DateTime? EventDate { get; set; }
        public string EventParticipantsJson { get; set; }
        public EventAction Action { get; set; }
        public string ServiceId { get; set; }
    }
}
