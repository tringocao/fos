using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.FoodServices;
using Newtonsoft.Json;
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
    public class FoodController : ApiController
    {
        IFoodService _foodService;
        IFoodDtoMapper _foodDtoMapper;
        IFoodCategoryDtoMapper _foodCategoryDtoMapper;
        public FoodController(IFoodService foodService, IFoodDtoMapper foodDtoMapper, IFoodCategoryDtoMapper foodCategoryDtoMapper)
        {
            _foodService = foodService;
            _foodDtoMapper = foodDtoMapper;
            _foodCategoryDtoMapper = foodCategoryDtoMapper;
        }
        // GET: api/Food
        [HttpGet]
        [Route("GetFoodCatalogues")]
        public async Task<ApiResponse<List<FoodCategory>>> GetFoodCataloguesAsync(int IdService, int delivery_id)
        {
            try
            {
                _foodService.GetExternalServiceById(IdService);
                var list = await _foodService.GetFoodCataloguesFromDeliveryIdAsync(delivery_id);
                return ApiUtil<List<FoodCategory>>.CreateSuccessfulResult(
                    list.Select(f => _foodCategoryDtoMapper.ToDto(f)).ToList()
                );
            }
            catch (Exception e)
            {
                return ApiUtil<List<FoodCategory>>.CreateFailResult(e.ToString());
            }
        }

        // GET: api/Food/5
        [HttpGet]
        [Route("GetFood")]
        public async Task<ApiResponse<List<Food>>> GetFoodAsync(int IdService, int delivery_id, int dish_type_id)
        {
            try
            {
                _foodService.GetExternalServiceById(IdService);
                var list = await _foodService.GetFoodFromCatalogueAsync(delivery_id, dish_type_id);

                return ApiUtil<List<Food>>.CreateSuccessfulResult(
                    list.Select(f => _foodDtoMapper.ToDto(f)).ToList()

                );
            }
            catch (Exception e)
            {
                return ApiUtil<List<Food>>.CreateFailResult(e.ToString());
            }
        }

        // POST: api/Food
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Food/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Food/5
        public void Delete(int id)
        {
        }
    }
}