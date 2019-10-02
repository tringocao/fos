using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class FeedBack
    {
        public string DeliveryId { get; set; }
        public List<UserRating> Ratings { get; set; }
        public List<FeedbackDetail> FoodFeedbacks { get; set; }
    }
}
