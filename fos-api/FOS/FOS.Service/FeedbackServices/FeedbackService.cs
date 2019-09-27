using FOS.Model.Domain;
using FOS.Repositories.Mapping;
using FOS.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.FeedbackServices
{
    public class FeedbackService : IFeedbackService
    {
        private IFeedbackRepository _feedbackRepository;
        private IFeedbackMapper _feedbackMapper;
        public FeedbackService(IFeedbackRepository feedbackRepository, IFeedbackMapper feedbackMapper)
        {
            _feedbackRepository = feedbackRepository;
            _feedbackMapper = feedbackMapper;
        }

        public FeedBack GetFeedbackByDeliveryId(string DeliveryId)
        {
            return _feedbackMapper.MapToDomain(_feedbackRepository.GetById(DeliveryId));
        }

        public void RateRestaurant(string restaurantId, float rating)
        {
            throw new NotImplementedException();
        }
    }
}
