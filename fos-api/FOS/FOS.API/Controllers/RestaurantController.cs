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
        public async Task<ApiResponse<IEnumerable<int>>> GetIdsAsync(int idService, int province_id)
        {
            try
            {
                _restaurantService.GetExternalServiceById(idService);
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
        public async Task<ApiResponse<Restaurant>> GetByIdAsync(int idService, int province_id, int restaurant_id)
        {
            try
            {
                _restaurantService.GetExternalServiceById(idService);
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
        public async Task<ApiResponse<IEnumerable<int>>> GetByKeywordLimitAsync(int idService, int cityId, string keyword, int limit)
        {
            try
            {
                _restaurantService.GetExternalServiceById(idService);
                return ApiUtil<IEnumerable<int>>.CreateSuccessfulResult(
                  (await _restaurantService.GetRestaurantsByKeywordAsync(cityId, keyword)).Select(l => Int32.Parse(l.RestaurantId)).Take(limit)
                );
            }
            catch (Exception e)
            {
                return ApiUtil<IEnumerable<int>>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetMetadataForCategory")]
        public async Task<ApiResponse<List<CategoryGroup>>> GetMetadataCategoryAsync(int idService)
        {
            try
            {
                _restaurantService.GetExternalServiceById(idService);
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
        public async Task<ApiResponse<IEnumerable<int>>> PutCategorySearchAsync(int idService, int cityId, string keyword, [FromBody]CategoryGroup categories)
        {
            try
            {
                List<Restaurant> listR = new List<Restaurant>();
                _restaurantService.GetExternalServiceById(idService);
                if (categories.Categories == null) return ApiUtil<IEnumerable<int>>.CreateSuccessfulResult(
                   new int[] { }
                );
                if (categories.Categories.Count() < 1)
                {
                    var list = await _restaurantService.GetRestaurantsByKeywordAsync(cityId, keyword);
                    listR = list.Select(r => _restaurantDtoMapper.ToDto(r)).ToList();
                }
                else
                {
                    var list = await _restaurantService.GetRestaurantsByCategoriesKeywordAsync(cityId, categories.Categories.Select(c => _categoryDtoMapper.ToModel(c)).ToList(), keyword);
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
