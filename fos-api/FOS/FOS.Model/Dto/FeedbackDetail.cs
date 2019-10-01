using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class FeedbackDetail
    {
        public string FoodId { get; set; }
        public List<UserFeedback> UserFeedBacks { get; set; }
    }
}
