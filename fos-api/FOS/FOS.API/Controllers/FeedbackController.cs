using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.FeedbackServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/feedback")]
    public class FeedbackController : ApiController
    {
        private IFeedbackService _feedbackService;
        private IFeedbackDtoMapper _feedbackDtoMapper;
        public FeedbackController(IFeedbackService feedbackService, IFeedbackDtoMapper feedbackDtoMapper)
        {
            _feedbackService = feedbackService;
            _feedbackDtoMapper = feedbackDtoMapper;
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public ApiResponse<Model.Dto.FeedBack> GetFeedbackByDeliveryId(string id)
        {
            try
            {
                var feedbacks = _feedbackDtoMapper.ToDto(_feedbackService.GetFeedbackByDeliveryId(id));
                return ApiUtil<Model.Dto.FeedBack>.CreateSuccessfulResult(feedbacks);
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.FeedBack>.CreateFailResult(e.ToString());
            }
        }
        [HttpPost]
        [Route("rate")]
        public ApiResponse RateRestaurant([FromBody]Model.Dto.FeedBack feedBack)
        {
            try
            {
                _feedbackService.RateRestaurant(_feedbackDtoMapper.ToDomain(feedBack));
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("sendEmail/{eventId}")]
        public ApiResponse SendFeedbackEmail(string eventId)
        {
            try
            {
                _feedbackService.SendFeedbackEmail(eventId);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
