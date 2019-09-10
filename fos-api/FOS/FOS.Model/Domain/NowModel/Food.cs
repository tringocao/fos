using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class Food
    {
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("is_available")]
        public bool IsAvailable { get; set; }
        [JsonProperty("is_group_discount_item")]
        public bool IsGroupDiscountItem { get; set; }
        [JsonProperty("photos")]
        public List<Photo> Photos { get; set; }
        [JsonProperty("price")]
        public Price Price { get; set; }
        [JsonProperty("discount_price")]
        public DiscountPrice DiscountPrice { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("time")]
        public string Time { get; set; }
        [JsonProperty("options")]
        public string Options { get; set; }
        [JsonProperty("properties")]
        public string Properties { get; set; }
        [JsonProperty("quantity")]
        public string Quantity { get; set; }

    }
}
