using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public class FavoriteRestaurantDtoMapper : IFavoriteRestaurantDtoMapper
    {
        public Dto.FavoriteRestaurant ToDto(Domain.FavoriteRestaurant favoriteRestaurant)
        {
            throw new NotImplementedException();
        }
    }

    public interface IFavoriteRestaurantDtoMapper
    {
        Dto.FavoriteRestaurant ToDto(Model.Domain.FavoriteRestaurant favoriteRestaurant);

    }
}
