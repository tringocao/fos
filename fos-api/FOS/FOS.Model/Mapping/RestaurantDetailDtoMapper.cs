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
        public Dto.RestaurantDetail ToDto(Model.Domain.NowModel.DeliveryDetail deliveryDetail)
        {
            return new Dto.RestaurantDetail()
            {
                Rating = deliveryDetail.Rating.avg,
                TotalReview = deliveryDetail.Rating.total_review
            };
        }
    }


}
