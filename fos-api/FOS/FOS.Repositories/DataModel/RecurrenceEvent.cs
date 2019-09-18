using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.DataModel
{
    public class RecurrenceEvent
    {
        public string UserId { get; set; }
        public string UserMail { get; set; }
        public string StartDate { get; set; }
        public string StartTempDate { get; set; } // StartTempDate = StartDate + next reminder(option if weekly or monthly)
        public bool IsReminding { get; set; }//If thread is died before send email, the windown service will ignore until the thread is finished sucessfully
        public string EndDate { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public string TypeRepeat { get; set; }
    }
}
