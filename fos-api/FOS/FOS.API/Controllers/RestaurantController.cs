using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Model.Params;
using FOS.Model.Util;
using FOS.Services.ExternalServices;
using FOS.Services.RestaurantServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [RoutePrefix("api/Restaurant")]

    public class RestaurantController : ApiController
    {
        IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
        // GET: api/Restaurant
        [HttpGet]
        [Route("GetIds")]
        public async Task<ApiResponse<IEnumerable<int>>> GetIdsAsync(int IdService, int province_id)
        {
            try
            {
                _restaurantService.GetExternalServiceById(IdService);
                return ApiUtil<IEnumerable<int>>.CreateSuccessfulResult(
                  (await  _restaurantService.GetRestaurantsByProvinceAsync(province_id)).Select(l => Int32.Parse(l.restaurant_id))
                );
            }
            catch (Exception e)
            {
                return ApiUtil<IEnumerable<int>>.CreateFailResult(e.ToString());
            }
        }

        // GET: api/Restaurant/5
        [HttpGet]
        [Route("GetById")]
        public async Task<ApiResponse<Restaurant>> GetByIdAsync(int IdService, int province_id, int restaurant_id)
        {
            try
            {
                _restaurantService.GetExternalServiceById(IdService);
                return ApiUtil<Restaurant>.CreateSuccessfulResult(
                  await _restaurantService.GetRestaurantsByIdAsync(province_id, restaurant_id)
                );
            }
            catch (Exception e)
            {
                return ApiUtil<Restaurant>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetByKeywordLimit")]
        public async Task<ApiResponse<IEnumerable<int>>> GetByKeywordLimitAsync(int IdService, int city_id, string keyword, int limit)
        {
            try
            {
               _restaurantService.GetExternalServiceById(IdService);
                return ApiUtil<IEnumerable<int>>.CreateSuccessfulResult(
                  (await _restaurantService.GetRestaurantsByKeywordAsync(city_id, keyword)).Select(l => Int32.Parse(l.restaurant_id)).Take(limit)
                );
            }
            catch (Exception e)
            {
                return ApiUtil<IEnumerable<int>>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetMetadataForCategory")]
        public async Task<ApiResponse<List<RestaurantCategory>>> GetMetadataCategoryAsync(int IdService)
        {
            try
            {
                _restaurantService.GetExternalServiceById(IdService);
                return ApiUtil<List<RestaurantCategory>>.CreateSuccessfulResult(
                  await _restaurantService.GetMetadataForCategoryAsync()
                );
            }
            catch (Exception e)
            {
                return ApiUtil<List<RestaurantCategory>>.CreateFailResult(e.ToString());
            }
        }
        // POST: api/Restaurant
        public void Post([FromBody]string value)
        {
        }
        [HttpPut]
        [Route("PutCategorySearch")]
        public async Task<ApiResponse<IEnumerable<int>>> PutCategorySearchAsync(int IdService, int city_id, string keyword, [FromBody]Categories categories)
        {
            try
            {
                List<Restaurant> listR = new List<Restaurant>();
                _restaurantService.GetExternalServiceById(IdService);
                if (categories.categories == null) return ApiUtil<IEnumerable<int>>.CreateSuccessfulResult(
                   new int[] {}
                );
                if (categories.categories.Count() < 1)
                {
                    listR = await _restaurantService.GetRestaurantsByKeywordAsync(city_id, keyword);
                }
                List<RestaurantCategory> newList = new List<RestaurantCategory>();
                JsonDtoMapper<RestaurantCategory> map = new JsonDtoMapper<RestaurantCategory>();
                foreach (var category in categories.categories)//get the fisrt catalogue
                {
                    newList.Add(map.ToDto(category));
                }
                listR = await _restaurantService.GetRestaurantsByCategoriesKeywordAsync(city_id, newList, keyword);
                return ApiUtil<IEnumerable<int>>.CreateSuccessfulResult(
                    listR.Select(l => Int32.Parse(l.restaurant_id))
                );
            }
            catch (Exception e)
            {
                return ApiUtil<IEnumerable<int>>.CreateFailResult(e.ToString());
            }
        }

        // DELETE: api/Restaurant/5
        public void Delete(int id)
        {
        }
    }
}
