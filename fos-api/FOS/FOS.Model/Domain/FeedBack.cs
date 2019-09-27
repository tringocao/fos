using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class FeedBack
    {
        public string DeliveryId { get; set; }
        public Dictionary<string, float> Ratings { get; set; }
        public Dictionary<int, Dictionary<string, string>> FoodFeedbacks { get; set; }
    }
}
