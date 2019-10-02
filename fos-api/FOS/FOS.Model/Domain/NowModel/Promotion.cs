using ChoETL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class Promotion
    {
        [JsonProperty("discount_amount")]
        public string DiscountAmount { set; get; }
        [JsonProperty("discount_on_type")]
        public string DiscountOnType { set; get; }
        [JsonProperty("discount_value_type")]
        public string DiscountValueType { set; get; }
        [JsonProperty("expired")]
        public string Expired { set; get; }
        [JsonProperty("max_discount_amount")]
        public string MaxDiscountAmount { set; get; }
        [JsonProperty("min_order_amount")]
        public string MinOrderAmount { set; get; }


    }
}
