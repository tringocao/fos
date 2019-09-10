using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class EventListItem
    {
        public string EventTitle { get; set; }
        public string EventRestaurant { get; set; }
        public int EventMaximumBudget { get; set; }
        public DateTime EventTimeToClose { get; set; }
        public DateTime EventTimeToReminder { get; set; }

        public string EventHost { get; set; }
        public string EventParticipants { get; set; }


        public string EventRestaurantId { get; set; }
        public string EventServiceId { get; set; }
        public string EventCategory { get; set; }

        public string EventDeliveryId { get; set; }
        public string EventCreatedUserId { get; set; }
        public string EventHostId { get; set; }
    }
}
