using FOS.Model.Domain;
using FOS.Repositories.Repositories;
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
    public class FavoriteService : IFavoriteService
    {
        IFOSFavoriteRestaurantRepository _fOSFavoriteRestaurantRepository;

        public FavoriteService(IFOSFavoriteRestaurantRepository fOSFavoriteRestaurantRepository)
        {
            _fOSFavoriteRestaurantRepository = fOSFavoriteRestaurantRepository;
        }

        public List<FavoriteRestaurant> GetFavoriteRestaurants()
        {
            return _fOSFavoriteRestaurantRepository.GetAll().ToList();
        }

        public List<FavoriteRestaurant> GetFavoriteRestaurantsById(string UserId)
        {
            return _fOSFavoriteRestaurantRepository.GetAll().Where(fav => fav.UserId == UserId).ToList();
        }

        public void AddFavoriteRestaurant(FavoriteRestaurant favoriteRestaurant)
        {
            _fOSFavoriteRestaurantRepository.AddFavorite(favoriteRestaurant);
        }

        public void RemoveFavoriteRestaurant(FavoriteRestaurant favoriteRestaurant)
        {
            _fOSFavoriteRestaurantRepository.DeleteFavorite(favoriteRestaurant);
        }
    }
}
