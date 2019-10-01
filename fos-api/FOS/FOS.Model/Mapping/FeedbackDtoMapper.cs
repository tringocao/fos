using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public interface IFeedbackDtoMapper
    {
        Model.Domain.FeedBack ToDomain(Model.Dto.FeedBack feedBack);
        Model.Dto.FeedBack ToDto(Model.Domain.FeedBack feedBack);
    }
    public class FeedbackDtoMapper : IFeedbackDtoMapper
    {
        public Domain.FeedBack ToDomain(Dto.FeedBack feedBack)
        {
            return new Domain.FeedBack()
            {
                DeliveryId = feedBack.DeliveryId,
                Ratings = feedBack.Ratings.ToDictionary(rating => rating.UserId, rating => rating.Rating),
                FoodFeedbacks = feedBack.FoodFeedbacks.ToDictionary(
                    ffb => Int32.Parse(ffb.FoodId), ffb => ffb.UserFeedBacks.ToDictionary(ufb => ufb.UserId, ufb => ufb.Comment))
            };
        }

        public Dto.FeedBack ToDto(Domain.FeedBack feedBack)
        {
            return new Dto.FeedBack()
            {
                DeliveryId = feedBack.DeliveryId,
                Ratings = feedBack.Ratings.Select(rating => new UserRating()
                {
                    UserId = rating.Key,
                    Rating = rating.Value
                }).ToList(),
                FoodFeedbacks = feedBack.FoodFeedbacks.Select(fb => new FeedbackDetail()
                {
                    FoodId = fb.Key.ToString(),
                    UserFeedBacks = fb.Value.Select(ufb => new UserFeedback()
                    {
                        UserId = ufb.Key,
                        Comment = ufb.Value
                    }).ToList()
                }).ToList()
            };
        }
    }
}
