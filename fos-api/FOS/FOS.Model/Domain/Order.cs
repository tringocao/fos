using FOS.Model.Domain.NowModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string IdUser { get; set; }
        public string IdEvent { get; set; }
        public int IdRestaurant { get; set; }
        public int IdDelivery { get; set; }
        public bool IsOrdered { get; set; }
        public Dictionary<int, Dictionary<string, string>> FoodDetail { get; set; }

    }
}
