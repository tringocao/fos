using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class Restaurant
    {
        [JsonProperty("restaurant_id")]
        public string RestaurantId { get; set; }
        //public int delivery_id { get; set; }
        //public dynamic delivery_id { get; set; }
        [JsonProperty("delivery_id")]
        public string DeliveryId { get; set; }

    }
}
