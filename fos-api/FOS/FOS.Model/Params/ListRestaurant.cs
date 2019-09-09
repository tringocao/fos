using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class ListRestaurant
    {
        [JsonProperty("restaurant_id")]
        public IEnumerable<int> RestaurantIds { get; set; }
    }
}
