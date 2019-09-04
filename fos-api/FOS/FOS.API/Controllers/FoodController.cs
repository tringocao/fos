using FOS.Model.Domain;
using FOS.Model.Dto;
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
    public class FoodController : ApiController
    {
        IFoodService _foodService;
        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }
        // GET: api/Food
        [HttpGet]
        [Route("GetFoodCatalogues")]
        public async Task<ApiResponse<List<FoodCategory>>> GetFoodCataloguesAsync(int IdService, int delivery_id)
        {
            try
            {
                _foodService.GetExternalServiceById(IdService);
                return ApiUtil<List<FoodCategory>>.CreateSuccessfulResult(
                  await _foodService.GetFoodCataloguesFromDeliveryIdAsync(delivery_id)
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
                return ApiUtil<List<Food>>.CreateSuccessfulResult(
                  await _foodService.GetFoodFromCatalogueAsync(delivery_id, dish_type_id)
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
