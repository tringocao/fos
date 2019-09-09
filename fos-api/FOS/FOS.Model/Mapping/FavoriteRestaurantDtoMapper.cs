using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public class FavoriteRestaurantDtoMapper: IFavoriteRestaurantDtoMapper
    {

    }

    public interface IFavoriteRestaurantDtoMapper
    {
        Dto.FavoriteRestaurant ToDto(Model.Domain.NowModel.DeliveryInfos deliveryInfos);

    }
}
