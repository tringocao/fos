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
        private IOrderRepository _orderRepository;
        public FeedbackService(IFeedbackRepository feedbackRepository, IFeedbackMapper feedbackMapper, IOrderRepository orderRepository)
        {
            _feedbackRepository = feedbackRepository;
            _feedbackMapper = feedbackMapper;
            _orderRepository = orderRepository;
        }

        public FeedBack GetFeedbackByDeliveryId(string DeliveryId)
        {
            var _feedback = _feedbackRepository.GetById(DeliveryId);
            if (_feedback != null)
            {
                return _feedbackMapper.MapToDomain(_feedback);
            }
            return null;
        }

        public void RateRestaurant(FeedBack feedBack)
        {
            var _feedback = _feedbackRepository.GetById(feedBack.DeliveryId);
            if (_feedback != null)
            {
                var feedbackDomain = _feedbackMapper.MapToDomain(_feedback);
                var userId = feedBack.Ratings.First().Key;

                var rating = feedBack.Ratings.First().Value;
                
                feedbackDomain.Ratings[userId] = rating;
                feedBack.FoodFeedbacks.ToList().ForEach(fb =>
                {
                    var food = feedbackDomain.FoodFeedbacks.FirstOrDefault(f => f.Key == fb.Key).Value;
                    if (food == null)
                    {
                        feedbackDomain.FoodFeedbacks.Add(fb.Key, fb.Value);
                        //feedbackDomain.FoodFeedbacks.Add(fb.Key,);
                    }
                    else
                    {
                        var foodFeedBacks = feedbackDomain.FoodFeedbacks[fb.Key];
                        foodFeedBacks[userId] = fb.Value.First().Value;
                        feedbackDomain.FoodFeedbacks[fb.Key] = foodFeedBacks;
                    }
                });
                //feedbackDomain.FoodFeedbacks.Where

                _feedbackRepository.Update(_feedbackMapper.MapToDataModel(feedbackDomain));
                //_rating =  feedBack.Ratings.First();
            }
            else
            {
                _feedbackRepository.Update(_feedbackMapper.MapToDataModel(feedBack));
            }
        }
    }
}
