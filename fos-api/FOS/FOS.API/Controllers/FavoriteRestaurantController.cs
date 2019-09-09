using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Mapping;
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
        IFavoriteRestaurantDtoMapper _favoriteRestaurantDtoMapper;
        public FavoriteRestaurantController(IFavoriteService favoriteService, IFavoriteRestaurantDtoMapper favoriteRestaurantDtoMapper)
        {
            _favoriteService = favoriteService;
            _favoriteRestaurantDtoMapper = favoriteRestaurantDtoMapper;
        }

        // GET: api/favoriterestaurant/getall
        [HttpGet]
        [Route("GetAll")]
        public ApiResponse<List<Model.Dto.FavoriteRestaurant>> GetAll()
        {
            try
            {
                var favoriteRestaurants = _favoriteService.GetFavoriteRestaurants();
                var favoriteRestaurantsDto = favoriteRestaurants.Select(
                    favoriteRestaurant => _favoriteRestaurantDtoMapper.ToDto(favoriteRestaurant)
                ).ToList();
                return ApiUtil<List<Model.Dto.FavoriteRestaurant>>.CreateSuccessfulResult(favoriteRestaurantsDto);
            }
            catch (Exception e)
            {
                return ApiUtil<List<Model.Dto.FavoriteRestaurant>>.CreateFailResult(e.ToString());
            }
        }

        // GET: api/favoriterestaurant/GetAllById/{userId}
        [HttpGet]
        [Route("GetAllById/{userId}")]
        public ApiResponse<List<Model.Dto.FavoriteRestaurant>> GetAllById(string userId)
        {
            try
            {
                var favoriteRestaurants = _favoriteService.GetFavoriteRestaurantsById(userId);
                var favoriteRestaurantsDto = favoriteRestaurants.Select(
                    favoriteRestaurant => _favoriteRestaurantDtoMapper.ToDto(favoriteRestaurant)
                ).ToList();
                return ApiUtil<List<Model.Dto.FavoriteRestaurant>>.CreateSuccessfulResult(favoriteRestaurantsDto);
            }
            catch (Exception e)
            {
                return ApiUtil<List<Model.Dto.FavoriteRestaurant>>.CreateFailResult(e.ToString());
            }
        }

        // POST: api/favoriterestaurant/add/
        [HttpPost]
        [Route("add")]
        public ApiResponse Add([FromBody]Model.Dto.FavoriteRestaurant favoriteRestaurant)
        {
            try
            {
                _favoriteService.AddFavoriteRestaurant(_favoriteRestaurantDtoMapper.ToModel(favoriteRestaurant));
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
        public ApiResponse Remove([FromBody] Model.Dto.FavoriteRestaurant favoriteRestaurant)
        {
            try
            {
                _favoriteService.RemoveFavoriteRestaurant(_favoriteRestaurantDtoMapper.ToModel(favoriteRestaurant));
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
