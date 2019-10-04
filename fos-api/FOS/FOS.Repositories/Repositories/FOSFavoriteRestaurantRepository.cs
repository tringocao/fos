using AutoMapper;
using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public interface IFOSFavoriteRestaurantRepository
    {
        IEnumerable<FavoriteRestaurant> GetAll();
        void AddFavorite(FavoriteRestaurant favoriteRestaurant);
        void DeleteFavorite(FavoriteRestaurant favoriteRestaurant);
    }
    public class FOSFavoriteRestaurantRepository : IFOSFavoriteRestaurantRepository
    {
        private readonly FosContext _context;
        public FOSFavoriteRestaurantRepository(FosContext context)
        {
            _context = context;
        }

        public void AddFavorite(FavoriteRestaurant favoriteRestaurant)
        {
            var _favoriteRestaurant = Mapper.Map<FavoriteRestaurant, DataModel.FavoriteRestaurant>(favoriteRestaurant);
            _context.FavoriteRestaurants.Add(_favoriteRestaurant);
            _context.SaveChanges();
        }

        public void DeleteFavorite(FavoriteRestaurant favoriteRestaurant)
        {
            var _favoriteRestaurant = _context.FavoriteRestaurants.Where(
                fav => fav.RestaurantId == favoriteRestaurant.RestaurantId && fav.UserId == favoriteRestaurant.UserId).FirstOrDefault();
            //var _favoriteRestaurant = Mapper.Map<FavoriteRestaurant, DataModel.FavoriteRestaurant>(favoriteRestaurantEntity);
            if (_favoriteRestaurant != null)
            {
                _context.FavoriteRestaurants.Remove(_favoriteRestaurant);
                _context.SaveChanges();
            }
        }

        public IEnumerable<FavoriteRestaurant> GetAll()
        {
            var list = _context.FavoriteRestaurants.ToList();
            return Mapper.Map<IEnumerable<DataModel.FavoriteRestaurant>, IEnumerable<FavoriteRestaurant>>(list);
        }
    }
}
