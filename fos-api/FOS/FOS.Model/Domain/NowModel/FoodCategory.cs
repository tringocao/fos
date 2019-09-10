using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class FoodCategory
    {
        [JsonProperty("dish_type_id")]
        public string DishTypeId { get; set; }
        [JsonProperty("dish_type_name")]
        public string DishTypeName { get; set; }
        [JsonProperty("dishes")]
        public List<Food> Dishes { get; set; }
    }

}