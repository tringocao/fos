using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class ExcelModel
    {
        public Event Event { get; set; }
        public RestaurantExcel RestaurantExcel { get; set; }
        public List<FoodReport> FoodReport { get; set; }
        public List<UserOrder> UserOrder { get; set; }
        public List<User> User { get; set; }
        public int Total { get; set; }
    }
}
