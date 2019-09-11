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
            return new Dto.FavoriteRestaurant()
            {
                RestaurantId = favoriteRestaurant.RestaurantId,
            };
        }

        public Domain.FavoriteRestaurant ToModel(Dto.FavoriteRestaurant favoriteRestaurant, string userId)
        {
            return new Domain.FavoriteRestaurant()
            {
                RestaurantId = favoriteRestaurant.RestaurantId,
                UserId = userId
            };
        }
    }

    public interface IFavoriteRestaurantDtoMapper
    {
        Dto.FavoriteRestaurant ToDto(Model.Domain.FavoriteRestaurant favoriteRestaurant);
        Domain.FavoriteRestaurant ToModel(Dto.FavoriteRestaurant favoriteRestaurant, string userId);
    }
}
