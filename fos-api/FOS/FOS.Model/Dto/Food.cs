using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class Food
    {
        public string description { get; set; }
        public int id { get; set; }
        public bool is_available { get; set; }
        public bool is_group_discount_item { get; set; }
        public string photos { get; set; }
        public Price price { get; set; }
        public DiscountPrice discount_price { get; set; }
        public string name { get; set; }
        public string time { get; set; }
        public string options { get; set; }
        public string properties { get; set; }
        public string quantity { get; set; }

    }
}
