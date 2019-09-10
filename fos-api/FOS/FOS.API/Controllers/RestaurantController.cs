using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Model.Mapping;
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
using FOS.Model.Mapping;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/Restaurant")]

    public class RestaurantController : ApiController
    {
        IRestaurantService _restaurantService;
        IRestaurantDetailDtoMapper _restaurantDetailDtoMapper;
        ICategoryGroupDtoMapper _categoryGroupDtoMapper;
        ICategoryDtoMapper _categoryDtoMapper;
        IRestaurantDtoMapper _restaurantDtoMapper;
        public RestaurantController(IRestaurantService restaurantService,
            IRestaurantDetailDtoMapper restaurantDetailDtoMapper,
            ICategoryGroupDtoMapper categoryGroupDtoMapper,
            ICategoryDtoMapper categoryDtoMapper,
            IRestaurantDtoMapper restaurantDtoMapper)
        {
            _restaurantDetailDtoMapper = restaurantDetailDtoMapper;
            _categoryGroupDtoMapper = categoryGroupDtoMapper;
            _categoryDtoMapper = categoryDtoMapper;
            _restaurantService = restaurantService;
            _restaurantDtoMapper = restaurantDtoMapper;
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
                  (await _restaurantService.GetRestaurantsByProvinceAsync(province_id)).Select(l => Int32.Parse(l.RestaurantId))
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
                  _restaurantDtoMapper.ToDto(await _restaurantService.GetRestaurantsByIdAsync(province_id, restaurant_id))
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
                  (await _restaurantService.GetRestaurantsByKeywordAsync(city_id, keyword)).Select(l => Int32.Parse(l.RestaurantId)).Take(limit)
                );
            }
            catch (Exception e)
            {
                return ApiUtil<IEnumerable<int>>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetMetadataForCategory")]
        public async Task<ApiResponse<List<CategoryGroup>>> GetMetadataCategoryAsync(int IdService)
        {
            try
            {
                _restaurantService.GetExternalServiceById(IdService);
                var list = await _restaurantService.GetMetadataForCategoryAsync();
                return ApiUtil<List<CategoryGroup>>.CreateSuccessfulResult(
                  list.Select(c => _categoryGroupDtoMapper.ToDto(c)).ToList()
                );
            }
            catch (Exception e)
            {
                return ApiUtil<List<CategoryGroup>>.CreateFailResult(e.ToString());
            }
        }
        // POST: api/Restaurant
        public void Post([FromBody]string value)
        {
        }
        [HttpPut]
        [Route("PutCategorySearch")]
        public async Task<ApiResponse<IEnumerable<int>>> PutCategorySearchAsync(int IdService, int city_id, string keyword, [FromBody]CategoryGroup categories)
        {
            try
            {
                List<Restaurant> listR = new List<Restaurant>();
                _restaurantService.GetExternalServiceById(IdService);
                if (categories.Categories == null) return ApiUtil<IEnumerable<int>>.CreateSuccessfulResult(
                   new int[] { }
                );
                if (categories.Categories.Count() < 1)
                {
                    var list = await _restaurantService.GetRestaurantsByKeywordAsync(city_id, keyword);
                    listR = list.Select(r => _restaurantDtoMapper.ToDto(r)).ToList();
                }
                else
                {
                    var list = await _restaurantService.GetRestaurantsByCategoriesKeywordAsync(city_id, categories.Categories.Select(c => _categoryDtoMapper.ToModel(c)).ToList(), keyword);
                    listR = list.Select(r => _restaurantDtoMapper.ToDto(r)).ToList();
                }
                return ApiUtil<IEnumerable<int>>.CreateSuccessfulResult(
                    listR.Select(l =>l.Id)
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
