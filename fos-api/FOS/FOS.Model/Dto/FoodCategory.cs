using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class FoodCategory
    {
        public string dish_type_id { get; set; }
        public string dish_type_name { get; set; }
        public List<Food> dishes { get; set; }
    }

}