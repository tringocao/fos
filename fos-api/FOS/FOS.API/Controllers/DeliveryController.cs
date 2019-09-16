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
        public async Task<ApiResponse<List<DeliveryInfos>>> GetAsync(int idService, int cityId, int restaurantId)
        {
            try
            {
                _deliveryService.GetExternalServiceById(idService);
                var list = await _deliveryService.GetRestaurantDeliveryInforAsync(cityId, restaurantId);
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
        public async Task<ApiResponse<DeliveryInfos>> GetFirstIdAsync(int idService, int cityId, int restaurantId)
        {
            try
            {
                _deliveryService.GetExternalServiceById(idService);
                var delivery = await _deliveryService.GetRestaurantFirstDeliveryInforAsync(cityId, restaurantId);

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
        public async Task<ApiResponse<RestaurantDetail>> GetRestaurantDetailAsync(int idService, int deliveryId)
        {
            try
            {
                _deliveryService.GetExternalServiceById(idService);
                return ApiUtil<RestaurantDetail>.CreateSuccessfulResult(
                   _restaurantDetailDtoMapper.ToDto(await _deliveryService.GetDetailAsync(deliveryId))
                );
            }
            catch (Exception e)
            {
                return ApiUtil<RestaurantDetail>.CreateFailResult(e.ToString());
            }

        }
        [HttpGet]
        [Route("GetPageDelivery")]
        public async Task<ApiResponse<List<DeliveryInfos>>> GetPageDeliveryAsync(int idService, int cityId, int pagenum, int pagesize)
        {
            try
            {
                _deliveryService.GetExternalServiceById(idService);
                var list = await _deliveryService.GetRestaurantDeliveryInforByPagingAsync(cityId, pagenum, pagesize);
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
        public async Task<ApiResponse<List<DeliveryInfos>>> Put(int idService, int cityId, [FromBody]List<Restaurant> data)
        {
            try
            {
                _deliveryService.GetExternalServiceById(idService);
                List<Model.Domain.NowModel.Restaurant> newList = new List<Model.Domain.NowModel.Restaurant>();

                if (data.Count() < 1) ApiUtil<List<DeliveryInfos>>.CreateFailResult("");
     
                foreach (var id in data)//get the fisrt catalogue.
                {
                    Model.Domain.NowModel.Restaurant item = new Model.Domain.NowModel.Restaurant();
                    item.RestaurantId = id.Id.ToString();
                    newList.Add(item);
                }
                var list = await _deliveryService.GetRestaurantsDeliveryInforAsync(cityId, newList);
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
