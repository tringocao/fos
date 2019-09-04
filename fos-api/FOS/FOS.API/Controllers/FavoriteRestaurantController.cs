using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Util;
using FOS.Services.FavoriteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
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
        public ApiResponse<List<FavoriteRestaurant>> GetAll()
        {
            try
            {
                var favoriteRestaurants = _favoriteService.GetFavoriteRestaurants();
                return ApiUtil<List<FavoriteRestaurant>>.CreateSuccessfulResult(favoriteRestaurants);
            }
            catch (Exception e)
            {
                return ApiUtil<List<FavoriteRestaurant>>.CreateFailResult(e.ToString());
            }
        }

        // GET: api/favoriterestaurant/GetAllById/{userId}
        [HttpGet]
        [Route("GetAllById/{userId}")]
        public ApiResponse<List<FavoriteRestaurant>> GetAllById(string userId)
        {
            try
            {
                var favoriteRestaurants = _favoriteService.GetFavoriteRestaurantsById(userId);
                return ApiUtil<List<FavoriteRestaurant>>.CreateSuccessfulResult(favoriteRestaurants);
            }
            catch (Exception e)
            {
                return ApiUtil<List<FavoriteRestaurant>>.CreateFailResult(e.ToString());
            }
        }

        // POST: api/favoriterestaurant/add/
        [HttpPost]
        [Route("add")]
        public ApiResponse Add([FromBody] FavoriteRestaurant favoriteRestaurant)
        {
            try
            {
                _favoriteService.AddFavoriteRestaurant(favoriteRestaurant);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }       
        }

        // POST: api/favoriterestaurant/remove/
        [HttpPost]
        [Route("remove")]
        public ApiResponse Remove([FromBody] FavoriteRestaurant favoriteRestaurant)
        {
            try
            {
                _favoriteService.RemoveFavoriteRestaurant(favoriteRestaurant);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
