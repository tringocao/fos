using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class EventListItem
    {
        public string eventTitle { get; set; }
        public string eventRestaurant { get; set; }
        public int eventMaximumBudget { get; set; }
        public DateTime eventTimeToClose { get; set; }
        public DateTime eventTimeToReminder { get; set; }

        public string eventHost { get; set; }
        public string eventParticipants { get; set; }


        public string eventRestaurantId { get; set; }
        public string eventServiceId { get; set; }
        public string eventCategory { get; set; }

        public string eventDeliveryId { get; set; }
        public string eventCreatedUserId { get; set; }
        public string eventHostId { get; set; }
    }
}
