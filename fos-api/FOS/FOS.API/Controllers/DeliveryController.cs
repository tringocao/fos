using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Dto;
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
        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }
        // GET: api/Delivery
        [HttpGet]
        [Route("Get")]
        public async Task<ApiResponse<List<DeliveryInfos>>> GetAsync(int IdService, int city_id, int restaurant_id)
        {
            try
            {
                _deliveryService.GetExternalServiceById(IdService);
                return ApiUtil<List<DeliveryInfos>>.CreateSuccessfulResult(
                  await _deliveryService.GetRestaurantDeliveryInforAsync(city_id, restaurant_id)
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
                return ApiUtil<DeliveryInfos>.CreateSuccessfulResult(
                  await _deliveryService.GetRestaurantFirstDeliveryInforAsync(city_id, restaurant_id)
                );
            }
            catch (Exception e)
            {
                return ApiUtil<DeliveryInfos>.CreateFailResult(e.ToString());
            }

        }
        [HttpGet]
        [Route("GetDeliveryDetail")]
        public async Task<ApiResponse<DeliveryDetail>> GetRestaurantDetailAsync(int IdService, int delivery_id)
        {
            try
            {
                _deliveryService.GetExternalServiceById(IdService);
                return ApiUtil<DeliveryDetail>.CreateSuccessfulResult(
                  await _deliveryService.GetRestaurantDetailAsync(new Restaurant() { delivery_id = delivery_id.ToString()})
                );
            }
            catch (Exception e)
            {
                return ApiUtil<DeliveryDetail>.CreateFailResult(e.ToString());
            }

        }
        [HttpGet]
        [Route("GetPageDelivery")]
        public async Task<ApiResponse<List<DeliveryInfos>>> GetPageDeliveryAsync(int IdService, int city_id, int pagenum, int pagesize)
        {
            try
            {
                _deliveryService.GetExternalServiceById(IdService);
                return ApiUtil<List<DeliveryInfos>>.CreateSuccessfulResult(
                  await _deliveryService.GetRestaurantDeliveryInforByPagingAsync(city_id, pagenum, pagesize)
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
                List<Restaurant> newList = new List<Restaurant>();

                if (data.restaurant_ids.Count() < 1) ApiUtil<List<DeliveryInfos>>.CreateFailResult("");
     
                foreach (var id in data.restaurant_ids)//get the fisrt catalogue
                {
                    Restaurant item = new Restaurant();
                    item.restaurant_id = id.ToString();
                    newList.Add(item);
                }
                    return ApiUtil<List<DeliveryInfos>>.CreateSuccessfulResult(
                  await _deliveryService.GetRestaurantsDeliveryInforAsync(city_id, newList)
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
