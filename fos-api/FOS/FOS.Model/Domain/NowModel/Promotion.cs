using ChoETL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class Promotion
    {
        [ChoJSONRecordField(JSONPath = "$.discount")]
        public string Discount { set; get; }
        [ChoJSONRecordField(JSONPath = "$.discount_amount")]
        public string DiscountAmount { set; get; }
        [ChoJSONRecordField(JSONPath = "$.discount_on_type")]
        public string DiscountOnType { set; get; }
        [ChoJSONRecordField(JSONPath = "$.discount_value_type")]
        public string DiscountValueType { set; get; }

        [ChoJSONRecordField(JSONPath = "$.expired")]
        public string Expired { set; get; }
        [ChoJSONRecordField(JSONPath = "$.max_discount_amount")]
        public string MaxDiscountAmount { set; get; }
        [ChoJSONRecordField(JSONPath = "$.min_order_amount")]
        public string MinOrderAmount { set; get; }


    }
}
