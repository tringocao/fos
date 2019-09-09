using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class FoodCategory
    {
        public string DishTypeName { get; set; }
        public string DishTypeId { get; set; }
        public List<Food> Dishes { get; set; }

    }
}
