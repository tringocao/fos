using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public class RestaurantDtoMapper : IRestaurantDtoMapper
    {
        public Dto.Restaurant ToDto(Domain.NowModel.Restaurant restaurant)
        {
            return new Dto.Restaurant()
            {
                DeliveryId = restaurant.DeliveryId,
                Id = Int32.Parse(restaurant.RestaurantId)
            };
        }
    }

    public interface IRestaurantDtoMapper
    {
        Dto.Restaurant ToDto(Model.Domain.NowModel.Restaurant restaurant);

    }
}
