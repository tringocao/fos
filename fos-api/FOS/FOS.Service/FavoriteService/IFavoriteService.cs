using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.FavoriteService
{
    public interface IFavoriteService
    {
        List<FavoriteRestaurant> GetFavoriteRestaurants();
        List<FavoriteRestaurant> GetFavoriteRestaurantsById(string UserId);
        void AddFavoriteRestaurant(FavoriteRestaurant favoriteRestaurant);
        void RemoveFavoriteRestaurant(FavoriteRestaurant favoriteRestaurant);
    }
}
