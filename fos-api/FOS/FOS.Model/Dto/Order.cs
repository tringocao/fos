using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int IdUser { get; set; }
        public Dictionary<string, Dictionary<string, string>> FoodDetail { get; set; }

    }
}
