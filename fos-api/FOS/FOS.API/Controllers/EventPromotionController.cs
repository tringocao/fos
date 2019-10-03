using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.DeliveryServices;
using FOS.Services.EventPromotionServices;
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
    [RoutePrefix("api/EventPromotion")]
    public class EventPromotionController : ApiController
    {
        IEventPromotionService _eventPromotionService;
        IEventPromotionDtoMapper _eventPromotionDtoMapper;
        IDeliveryService _deliveryService;
        IDeliveryInfosDtoMapper _deliveryInfosDtoMapper;
        IRestaurantDetailDtoMapper _restaurantDetailDtoMapper;
        IPromotionDtoMapper _promotionDtoMapper;
        public EventPromotionController(IEventPromotionService discountEventService,
            IEventPromotionDtoMapper discountEventDtoMapper,
            IDeliveryService deliveryService,
            IDeliveryInfosDtoMapper deliveryInfosDtoMapper,
            IRestaurantDetailDtoMapper restaurantDetailDtoMapper,
            IPromotionDtoMapper promotionDtoMapper)
        {
            _eventPromotionService = discountEventService;
            _eventPromotionDtoMapper = discountEventDtoMapper;
            _deliveryService = deliveryService;
            _deliveryInfosDtoMapper = deliveryInfosDtoMapper;
            _restaurantDetailDtoMapper = restaurantDetailDtoMapper;
            _promotionDtoMapper = promotionDtoMapper;
        }
        [HttpGet]
        [Route("GetPromotionsByExternalService")]
        public async Task<ApiResponse<IEnumerable<Promotion>>> GetPromotionsByExternalServiceAsync(int idService, int deliveryId)
        {
            try
            {
                _deliveryService.GetExternalServiceById(idService);
                var restaurantDetail = _restaurantDetailDtoMapper.ToDto(await _deliveryService.GetDetailAsync(deliveryId));

                return ApiUtil<IEnumerable<Promotion>>.CreateSuccessfulResult(restaurantDetail.PromotionLists);
            }
            catch (Exception e)
            {
                return ApiUtil<IEnumerable<Promotion>>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetById")]
        public ApiResponse<Model.Dto.EventPromotion> GetById(int id)
        {
            try
            {
                return ApiUtil<Model.Dto.EventPromotion>.CreateSuccessfulResult(
                    _eventPromotionDtoMapper.ToDto(_eventPromotionService.GetById(id))
               );
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.EventPromotion>.CreateFailResult(e.ToString());
            }
        }

        [HttpGet]
        [Route("GetByEventId")]
        public ApiResponse<Model.Dto.EventPromotion> GetByEventId(int eventId)
        {
            try
            {
                var eventPromotion = _eventPromotionService.GetByEventId(eventId);
                if (eventPromotion != null)
                {
                    return ApiUtil<Model.Dto.EventPromotion>.CreateSuccessfulResult(
                        _eventPromotionDtoMapper.ToDto(eventPromotion)
                    );
                }
                return ApiUtil<Model.Dto.EventPromotion>.CreateSuccessfulResult(
                    null
                );
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.EventPromotion>.CreateFailResult(e.ToString());
            }
        }
        [HttpPut]
        [Route("UpdateEventPromotionByEventId")]
        public ApiResponse UpdateEventPromotionByEventId(int eventId, [FromBody]List<Model.Dto.Promotion> newPromotions)
        {
            try
            {
                _eventPromotionService.UpdateEventPromotionByEventId(eventId, newPromotions.Select(p => _promotionDtoMapper.ToModel(p)).ToList());


                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpPost]
        [Route("AddEventPromotion")]
        public ApiResponse AddEventPromotion(int eventId, [FromBody]List<Model.Dto.Promotion> newPromotions)
        {
            try
            {
                Model.Dto.EventPromotion eventPromotion = new Model.Dto.EventPromotion();
                eventPromotion.EventId = eventId;
                eventPromotion.Promotions = newPromotions;
                _eventPromotionService.AddEventPromotion(_eventPromotionDtoMapper.ToModel(eventPromotion));


                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpPost]
        [Route("UpdateEventPromotion")]
        public ApiResponse UpdateEventPromotion([FromBody]Model.Dto.EventPromotion eventPromotion)
        {
            try
            {
                _eventPromotionService.UpdateEventPromotion(_eventPromotionDtoMapper.ToModel(eventPromotion));


                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
