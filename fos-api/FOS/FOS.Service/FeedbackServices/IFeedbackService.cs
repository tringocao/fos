using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.FeedbackServices
{
    public interface IFeedbackService
    {
        Model.Domain.FeedBack GetFeedbackByDeliveryId(string DeliveryId);
        void RateRestaurant(string restaurantId, float rating);
    }
}
