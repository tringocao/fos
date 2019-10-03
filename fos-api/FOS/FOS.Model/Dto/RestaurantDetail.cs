using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
   public class RestaurantDetail
    {
        public float Rating { get; set; }
        public int TotalReview { get; set; }
        public List<Promotion> PromotionLists { get; set; }
    }
}
