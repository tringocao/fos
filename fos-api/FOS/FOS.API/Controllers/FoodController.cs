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
        IPromotionDtoMapper _promotionDtoMapper;
        public FoodController(IFoodService foodService, IFoodDtoMapper foodDtoMapper, IFoodCategoryDtoMapper foodCategoryDtoMapper,IPromotionDtoMapper promotionDtoMapper)
        {
            _foodService = foodService;
            _foodDtoMapper = foodDtoMapper;
            _foodCategoryDtoMapper = foodCategoryDtoMapper;
            _promotionDtoMapper = promotionDtoMapper;
        }
        // GET: api/Food
        [HttpGet]
        [Route("GetFoodCatalogues")]
        public async Task<ApiResponse<List<FoodCategory>>> GetFoodCataloguesAsync(int idService, int deliveryId)
        {
            try
            {
                _foodService.GetExternalServiceById(idService);
                var list = await _foodService.GetFoodCataloguesFromDeliveryIdAsync(deliveryId);
                return ApiUtil<List<FoodCategory>>.CreateSuccessfulResult(
                    list.Select(f => _foodCategoryDtoMapper.ToDto(f)).ToList()
                );
            }
            catch (Exception e)
            {
                return ApiUtil<List<FoodCategory>>.CreateFailResult(e.ToString());
            }
        }
        [HttpPut]
        [Route("GetDiscountedFoodIds")]
        public async Task<ApiResponse<Model.Dto.Promotion>> GetDiscountedFoodIds(int idService, int deliveryId,[FromBody] Model.Dto.Promotion promotion)
        {
            try
            {
                _foodService.GetExternalServiceById(idService);
                var newPromotion = await _foodService.GetDiscountedFoodIds(deliveryId, _promotionDtoMapper.ToModel(promotion));
                return ApiUtil<Promotion>.CreateSuccessfulResult(_promotionDtoMapper.ToDto(newPromotion));
            }
            catch (Exception e)
            {
                return ApiUtil<Promotion>.CreateFailResult(e.ToString());
            }
        }
        // GET: api/Food/5
        [HttpGet]
        [Route("GetFood")]
        public async Task<ApiResponse<List<Food>>> GetFoodAsync(int idService, int deliveryId, int dishTypeId)
        {
            try
            {
                _foodService.GetExternalServiceById(idService);
                var list = await _foodService.GetFoodFromCatalogueAsync(deliveryId, dishTypeId);

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