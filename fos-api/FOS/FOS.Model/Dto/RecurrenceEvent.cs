using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class RecurrenceEvent
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public RepeateType TypeRepeat { get; set; }
        public string UserId { get; set; }
        public string UserMail { get; set; }

    }
}
