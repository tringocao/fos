using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class RecurrenceEvent
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTempDate { get; set; } // StartTempDate = StartTempDate + next reminder(option if weekly or monthly)
        public string UserMail { get; set; }

        public int Version { get; set; }

        public string Title { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }

        public RepeateType TypeRepeat { get; set; }
    }
}
