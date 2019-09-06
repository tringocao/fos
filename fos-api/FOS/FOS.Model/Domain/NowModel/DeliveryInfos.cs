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
        public string CityId { get; set; }
        [JsonProperty("restaurant_id")]
        public string RestaurantId { get; set; }
        [JsonProperty("is_open")]
        public string IsOpen { get; set; }
        [JsonProperty("is_foody_delivery")]
        public string IsFoodyDelivery { get; set; }
        [JsonProperty("campaigns")]
        public string Campaigns { get; set; }
        [JsonProperty("promotion_groups")]
        public string PromotionGroups { get; set; }
        [JsonProperty("photos")]
        public List<Photo> Photos { get; set; }
        [JsonProperty("operating")]
        public string Operating { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("delivery_id")]
        public string DeliveryId { get; set; }
        [JsonProperty("categories")]
        public string Categories { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url_rewrite_name")]
        public string UrlRewriteName { get; set; }
    }
}
