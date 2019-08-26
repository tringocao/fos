using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class Event
    {
        public string Title { get; set; }
        public string EventId { get; set; }

        public Event(string _title, string _eventId)
        {
            Title = _title;
            EventId = _eventId;
        }
    }
}
