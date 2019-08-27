using FOS.Model.Domain;
using FOS.Services.FavoriteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    public class FavoriteRestaurantController : ApiController
    {
        IFavoriteService _favoriteService;
        public FavoriteRestaurantController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // GET: api/favoriterestaurant/getall
        public IEnumerable<FavoriteRestaurant> GetAll()
        {
            return _favoriteService.GetFavoriteRestaurants();
        }

        // GET: api/favoriterestaurant/GetAllById/id
        public IEnumerable<FavoriteRestaurant> GetAllById(string userId)
        {
            return _favoriteService.GetFavoriteRestaurantsById(userId);
        }

        // POST: api/favoriterestaurant/add/id
        public void Add([FromBody] FavoriteRestaurant favoriteRestaurant)
        {
            _favoriteService.AddFavoriteRestaurant(favoriteRestaurant);
        }

        // DELETE: api/favoriterestaurant/remove/id
        public void Remove([FromBody] FavoriteRestaurant favoriteRestaurant)
        {
            _favoriteService.RemoveFavoriteRestaurant(favoriteRestaurant);
        }
    }
}
