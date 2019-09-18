using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.DataModel
{
    public class Order
    {
        public string Id { get; set; }
        public string OrderDate { get; set; }
        public string IdUser { get; set; }
        public string IdEvent { get; set; }
        public int IdRestaurant { get; set; }
        public int IdDelivery { get; set; }
        public bool IsOrdered { get; set; }
        public string FoodDetail { get; set; }
        public string Email { get; set; }
    }
}
