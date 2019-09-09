using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.DeliveryServices;
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
    [LogActionWebApiFilter]
    [RoutePrefix("api/Delivery")]
    public class DeliveryController : ApiController
    {
        IDeliveryService _deliveryService;
        IDeliveryInfosDtoMapper _deliveryInfosDtoMapper;
        IRestaurantDetailDtoMapper _restaurantDetailDtoMapper;
        public DeliveryController(IDeliveryService deliveryService, IDeliveryInfosDtoMapper deliveryInfosDtoMapper, IRestaurantDetailDtoMapper restaurantDetailDtoMapper)
        {
            _deliveryService = deliveryService;
            _deliveryInfosDtoMapper = deliveryInfosDtoMapper;
            _restaurantDetailDtoMapper = restaurantDetailDtoMapper;
        }
        // GET: api/Delivery
        [HttpGet]
        [Route("Get")]
        public async Task<ApiResponse<List<DeliveryInfos>>> GetAsync(int IdService, int city_id, int restaurant_id)
        {
            try
            {
                _deliveryService.GetExternalServiceById(IdService);
                var list = await _deliveryService.GetRestaurantDeliveryInforAsync(city_id, restaurant_id);
                return ApiUtil<List<DeliveryInfos>>.CreateSuccessfulResult(
                    list.Select(d => _deliveryInfosDtoMapper.ToDto(d)).ToList()
                );
            }
            catch (Exception e)
            {
                return ApiUtil<List<DeliveryInfos>>.CreateFailResult(e.ToString());
            }
        }

        // GET: api/Delivery/5
        [HttpGet]
        [Route("GetFirstId")]
        public async Task<ApiResponse<DeliveryInfos>> GetFirstIdAsync(int IdService, int city_id, int restaurant_id)
        {
            try
            {
                _deliveryService.GetExternalServiceById(IdService);
                var delivery = await _deliveryService.GetRestaurantFirstDeliveryInforAsync(city_id, restaurant_id);

                return ApiUtil<DeliveryInfos>.CreateSuccessfulResult(
                    _deliveryInfosDtoMapper.ToDto(delivery)
                ); 
            }
            catch (Exception e)
            {
                return ApiUtil<DeliveryInfos>.CreateFailResult(e.ToString());
            }

        }
        [HttpGet]
        [Route("GetDeliveryDetail")]
        public async Task<ApiResponse<RestaurantDetail>> GetRestaurantDetailAsync(int IdService, int delivery_id)
        {
            try
            {
                _deliveryService.GetExternalServiceById(IdService);
                return ApiUtil<RestaurantDetail>.CreateSuccessfulResult(
                   _restaurantDetailDtoMapper.ToDto(await _deliveryService.GetDetailAsync(delivery_id))
                );
            }
            catch (Exception e)
            {
                return ApiUtil<RestaurantDetail>.CreateFailResult(e.ToString());
            }

        }
        [HttpGet]
        [Route("GetPageDelivery")]
        public async Task<ApiResponse<List<DeliveryInfos>>> GetPageDeliveryAsync(int IdService, int city_id, int pagenum, int pagesize)
        {
            try
            {
                _deliveryService.GetExternalServiceById(IdService);
                var list = await _deliveryService.GetRestaurantDeliveryInforByPagingAsync(city_id, pagenum, pagesize);
                return ApiUtil<List<DeliveryInfos>>.CreateSuccessfulResult(
                  list.Select(d => _deliveryInfosDtoMapper.ToDto(d)).ToList()
                );
            }
            catch (Exception e)
            {
                return ApiUtil<List<DeliveryInfos>>.CreateFailResult(e.ToString());
            }
        }
        // POST: api/Delivery
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Delivery/5
        [HttpPut]
        [Route("PutRestaurantIds")]
        // PUT: api/Delivery/5
        public async Task<ApiResponse<List<DeliveryInfos>>> Put(int IdService, int city_id, [FromBody]ListRestaurant data)
        {
            try
            {
                _deliveryService.GetExternalServiceById(IdService);
                List<Model.Domain.NowModel.Restaurant> newList = new List<Model.Domain.NowModel.Restaurant>();

                if (data.RestaurantIds.Count() < 1) ApiUtil<List<DeliveryInfos>>.CreateFailResult("");
     
                foreach (var id in data.RestaurantIds)//get the fisrt catalogue.
                {
                    Model.Domain.NowModel.Restaurant item = new Model.Domain.NowModel.Restaurant();
                    item.RestaurantId = id.ToString();
                    newList.Add(item);
                }
                var list = await _deliveryService.GetRestaurantsDeliveryInforAsync(city_id, data);
                return ApiUtil<List<DeliveryInfos>>.CreateSuccessfulResult(
                    list.Select(d => _deliveryInfosDtoMapper.ToDto(d)).ToList()
                );
            }
            catch (Exception e)
            {
                return ApiUtil<List<DeliveryInfos>>.CreateFailResult(e.ToString());
            }
        }

        // DELETE: api/Delivery/5
        public void Delete(int id)
        {
        }
    }
}
