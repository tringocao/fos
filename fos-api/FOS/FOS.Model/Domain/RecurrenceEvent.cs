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
        public DateTime StartTempDate { get; set; } // StartTempDate = StartDate + next reminder(option if weekly or monthly)
        public string UserMail { get; set; }

        public bool IsReminding { get; set; }//If thread is died before send email, the windown service will ignore until the thread is finished sucessfully

        public string Title { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }

        public RepeateType TypeRepeat { get; set; }
    }
}
