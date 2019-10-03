using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public interface IRestaurantDetailDtoMapper
    {
        Dto.RestaurantDetail ToDto(Model.Domain.NowModel.DeliveryDetail deliveryDetail);

    }
    public class RestaurantDetailDtoMapper: IRestaurantDetailDtoMapper
    {
        IPromotionDtoMapper _promotionDtoMapper;
        public RestaurantDetailDtoMapper(IPromotionDtoMapper promotionDtoMapper)
        {
            _promotionDtoMapper = promotionDtoMapper;
        }
        public Dto.RestaurantDetail ToDto(Model.Domain.NowModel.DeliveryDetail deliveryDetail)
        {
            List<Dto.Promotion> promotions = new List<Dto.Promotion>();
            if(deliveryDetail.PromotionOnAll != null)
            {
                promotions.AddRange(deliveryDetail.PromotionOnAll.Select(p => _promotionDtoMapper.ToDto(p)).ToList());
            }
            if (deliveryDetail.PromotionOnItem != null)
            {
                promotions.Add(_promotionDtoMapper.ToDto(deliveryDetail.PromotionOnItem));
            }
            return new Dto.RestaurantDetail()
            {
                Rating = deliveryDetail.Rating.avg,
                TotalReview = deliveryDetail.Rating.total_review,
                PromotionLists = promotions
            };
        }
    }


}
