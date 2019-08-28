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
    [RoutePrefix("api/favorite")]
    public class FavoriteRestaurantController : ApiController
    {
        IFavoriteService _favoriteService;
        public FavoriteRestaurantController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // GET: api/favoriterestaurant/getall
        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<FavoriteRestaurant> GetAll()
        {
            return _favoriteService.GetFavoriteRestaurants();
        }

        // GET: api/favoriterestaurant/GetAllById/{userId}
        [HttpGet]
        [Route("GetAllById/{userId}")]
        public IEnumerable<FavoriteRestaurant> GetAllById(string userId)
        {
            return _favoriteService.GetFavoriteRestaurantsById(userId);
        }

        // POST: api/favoriterestaurant/add/
        [HttpPost]
        [Route("add")]
        public void Add([FromBody] FavoriteRestaurant favoriteRestaurant)
        {
            _favoriteService.AddFavoriteRestaurant(favoriteRestaurant);
        }

        // POST: api/favoriterestaurant/remove/
        [HttpPost]
        [Route("remove")]
        public void Remove([FromBody] FavoriteRestaurant favoriteRestaurant)
        {
            _favoriteService.RemoveFavoriteRestaurant(favoriteRestaurant);
        }
    }
}
