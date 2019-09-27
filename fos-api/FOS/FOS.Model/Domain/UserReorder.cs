using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class UserReorder
    {
        public string UserMail { get; set; }
        public string OrderId { get; set; }
        public string UserName { get; set; }

        public string EventTitle { get; set; }
        public string EventRestaurant { get; set; }
        public List<string> FoodName { get; set; }

    }
}
