using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class RestaurantSummary
    {
        public int Rank { get; set; }
        public string Restaurant { get; set; }
        public float Percent { get; set; }
        public float RelativePercent { get; set; }
        public string RestaurantId { get; set; }
        public string DeliveryId { get; set; }
        public string ServiceId { get; set; }
        public int AppearTime { get; set; }
    }
}
