using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class EventList
    {
        public string eventTitles { get; set; }
        public string eventId;
        public string eventRestaurant;
        public int eventMaximumBudget;
        public DateTime eventTimeToClose;
        public DateTime eventTimeToReminder;
        public string eventHost;
        public string eventParticipants;
        public string eventRestaurantId;
        public string eventServiceId;
        public int eventCategory;
    }
}
