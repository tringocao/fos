using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Common.Constants
{
    public class EventFieldName
    {
        public const string EventList = "Event List";
        public const string EventHost = "EventHost";
        public const string EventTimeToClose = "EventTimeToClose";
        public const string EventTimeToReminder = "EventTimeToReminder";
        public const string EventDate = "EventDate";
        public const string EventTitle = "EventTitle";
        public const string EventRestaurant = "EventRestaurant";
        public const string EventRestaurantId = "EventRestaurantId";
        public const string EventCategory = "EventCategory";
        public const string EventParticipants = "EventParticipants";
        public const string EventMaximumBudget = "EventMaximumBudget";
        public const string ID = "ID";
        public const string EventDeliveryId = "EventDeliveryId";
        public const string EventHostId = "EventHostId";
        public const string EventCreatedUserId = "EventCreatedUserId";
        public const string EventStatus = "EventStatus";
        public const string EventTypes = "EventTypes";
        public const string EventParticipantsJson = "EventParticipantsJson";
        public const string EventServiceId = "EventServiceId";
        public const string EventId = "EventId";
        public const string EventIsReminder = "EventIsReminder";
    }
    public class EventStatus
    {
        public const string Reopened = "Reopened";
        public const string Opened = "Opened";
        public const string Closed = "Closed";
        public const string Error = "Error";
    }
    public class EventType
    {
        public const string Open = "Open";
        public const string Close = "Close";
    }
    public class EventIsReminder
    {
        public const string Yes = "Yes";
        public const string No = "No";
    }
}
