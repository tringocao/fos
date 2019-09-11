using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.FavoriteService;
using FOS.Services.SPUserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/favorite")]
    public class FavoriteRestaurantController : ApiController
    {
        IFavoriteService _favoriteService;
        ISPUserService _spUserService;
        IFavoriteRestaurantDtoMapper _favoriteRestaurantDtoMapper;
        public FavoriteRestaurantController(IFavoriteService favoriteService, IFavoriteRestaurantDtoMapper favoriteRestaurantDtoMapper, ISPUserService spUserService)
        {
            _favoriteService = favoriteService;
            _favoriteRestaurantDtoMapper = favoriteRestaurantDtoMapper;
            _spUserService = spUserService;
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

        [HttpGet]
        [Route("GetAllByCurrentUser")]
        public async Task<ApiResponse<List<Model.Dto.FavoriteRestaurant>>> GetAllByCurrentUser()
        {
            try
            {
                var currrentUser = await _spUserService.GetCurrentUser();
                var favoriteRestaurants = _favoriteService.GetFavoriteRestaurantsById(currrentUser.Id);
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
        public async Task<ApiResponse> Add([FromBody]Model.Dto.FavoriteRestaurant favoriteRestaurant)
        {
            try
            {
                var currrentUser = await _spUserService.GetCurrentUser();
                _favoriteService.AddFavoriteRestaurant(_favoriteRestaurantDtoMapper.ToModel(favoriteRestaurant, currrentUser.Id));
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
        public async Task<ApiResponse> Remove([FromBody] Model.Dto.FavoriteRestaurant favoriteRestaurant)
        {
            try
            {
                var currrentUser = await _spUserService.GetCurrentUser();
                _favoriteService.RemoveFavoriteRestaurant(_favoriteRestaurantDtoMapper.ToModel(favoriteRestaurant, currrentUser.Id));
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
