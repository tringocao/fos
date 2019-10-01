using FOS.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class Promotion
    {
        public PromotionType PromotionType { set; get; }
        public int Value { set; get; }
        public bool IsPercent { set; get; }
        public DateTime? Expired { set; get; }
        public int? MaxDiscountAmount { set; get; }
        public int? MinOrderAmount { set; get; }
    }
}
