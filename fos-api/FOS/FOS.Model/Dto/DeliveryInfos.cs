using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class DeliveryInfos
    {
        public string city_id { get; set; }
        public dynamic cuisines { get; set; }
        public string restaurant_id { get; set; }
        public string is_open { get; set; }
        public string is_foody_delivery { get; set; }
        public dynamic campaigns { get; set; }
        public dynamic photos { get; set; }
        public dynamic operating { get; set; }

        public string address { get; set; }
        public string delivery_id { get; set; }
        public dynamic categories { get; set; }

        public string name { get; set; }
        public string url_rewrite_name { get; set; }
    }
}
