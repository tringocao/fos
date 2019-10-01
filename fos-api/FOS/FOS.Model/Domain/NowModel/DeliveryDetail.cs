using ChoETL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class DeliveryDetail
    {
        [ChoJSONRecordField]
        public dynamic Rating { get; set; }
        [ChoJSONRecordField(JSONPath = "$.delivery.promotions")]
        public List<Promotion> PromotionOnAll { get; set; }
        [ChoJSONRecordField(JSONPath = "$.price_slash_discount")]
        public Promotion PromotionOnItem { get; set; }
    }
}
