using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class UserNotOrderMailInfo
    {
        public string UserMail { get; set; }
        public string OrderId { get; set; }
        public string EventTitle { get; set; }
        public string EventRestaurant { get; set; }
        public string UserName { get; set; }
    }
}
