using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class DeliveryInfos
    {
        [JsonProperty("city_id")]
        public int CityId { get; set; }
        [JsonProperty("restaurant_id")]
        public int RestaurantId { get; set; }
        [JsonProperty("is_open")]
        public bool IsOpen { get; set; }
        [JsonProperty("is_foody_delivery")]
        public bool IsFoodyDelivery { get; set; }

        [JsonProperty("promotion_groups")]
        public List<PromotionGroup> PromotionGroups { get; set; }
        [JsonProperty("photos")]
        public List<Photo> Photos { get; set; }
        [JsonProperty("operating")]
        public Operating Operating { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("delivery_id")]
        public int DeliveryId { get; set; }
        [JsonProperty("categories")]
        public List<string> Categories { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url_rewrite_name")]
        public string UrlRewriteName { get; set; }
    }
}
