using AutoMapper;
using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public interface IFOSFavoriteRestaurantRepository
    {
        IEnumerable<FavoriteRestaurant> GetAll();
        bool AddFavorite(FavoriteRestaurant favoriteRestaurant);
        bool DeleteFavorite(FavoriteRestaurant favoriteRestaurant);
    }
    public class FOSFavoriteRestaurantRepository : IFOSFavoriteRestaurantRepository
    {
        private readonly FosContext _context;
        public FOSFavoriteRestaurantRepository(FosContext context)
        {
            _context = context;
        }

        public bool AddFavorite(FavoriteRestaurant favoriteRestaurant)
        {
            try
            {
                var _favoriteRestaurant = Mapper.Map<FavoriteRestaurant, DataModel.FavoriteRestaurant>(favoriteRestaurant);
                _context.FavoriteRestaurants.Add(_favoriteRestaurant);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool DeleteFavorite(FavoriteRestaurant favoriteRestaurant)
        {
            try
            {
                var _favoriteRestaurant = Mapper.Map<FavoriteRestaurant, DataModel.FavoriteRestaurant>(favoriteRestaurant);
                _context.FavoriteRestaurants.Remove(_favoriteRestaurant);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IEnumerable<FavoriteRestaurant> GetAll()
        {
            var list = _context.FavoriteRestaurants.ToList();
            return Mapper.Map<IEnumerable<DataModel.FavoriteRestaurant>, IEnumerable<FavoriteRestaurant>>(list);
        }
    }
}
