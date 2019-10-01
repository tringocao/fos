using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.EventPromotionServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/EventPromotion")]
    public class EventPromotionController : ApiController
    {
        IEventPromotionService _discountEventService;
        IEventPromotionDtoMapper _discountEventDtoMapper;
        public EventPromotionController(IEventPromotionService discountEventService, IEventPromotionDtoMapper discountEventDtoMapper)
        {
            _discountEventService = discountEventService;
            _discountEventDtoMapper = discountEventDtoMapper;
        }

        [HttpGet]
        [Route("GetById")]
        public ApiResponse<Model.Dto.EventPromotion> GetById(int id)
        {
            try
            {
                return ApiUtil<Model.Dto.EventPromotion>.CreateSuccessfulResult(
                    _discountEventDtoMapper.ToDto(_discountEventService.GetById(id))
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
                return ApiUtil<Model.Dto.EventPromotion>.CreateSuccessfulResult(
                    _discountEventDtoMapper.ToDto(_discountEventService.GetByEventId(eventId))
               );
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.EventPromotion>.CreateFailResult(e.ToString());
            }
        }
        [HttpPost]
        [Route("AddNew")]
        public ApiResponse AddNew([FromBody]Model.Dto.EventPromotion discountEvent)
        {
            try
            {
                _discountEventService.AddDiscountEvent(_discountEventDtoMapper.ToModel(discountEvent));
               

                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpPost]
        [Route("UpdateDiscountEvent")]
        public ApiResponse UpdateDiscountEvent([FromBody]Model.Dto.EventPromotion discountEvent)
        {
            try
            {
                _discountEventService.UpdateDiscountEvent(_discountEventDtoMapper.ToModel(discountEvent));

           
                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
