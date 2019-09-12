using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class DeliveryInfos
    {
        public string CityId { get; set; }
        public string RestaurantId { get; set; }
        public bool IsFavorite { get; set; }
        public string IsOpen { get; set; }
        public string IsFoodyDelivery { get; set; }
        public string Campaigns { get; set; }
        public string PromotionGroups { get; set; }
        public string Photo { get; set; }
        public string Operating { get; set; }
        public string Address { get; set; }
        public string DeliveryId { get; set; }
        public string Categories { get; set; }
        public string Name { get; set; }
        public string UrlRewriteName { get; set; }
    }
}
