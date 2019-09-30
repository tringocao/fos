using FOS.API.App_Start;
using FOS.Common.Constants;
using FOS.Model.Domain;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.FeedbackServices;
using FOS.Services.OrderServices;
using FOS.Services.SendEmailServices;
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
    [RoutePrefix("api/feedback")]
    public class FeedbackController : ApiController
    {
        private IFeedbackService _feedbackService;
        private IFeedbackDtoMapper _feedbackDtoMapper;
        private ISendEmailService _sendEmailService;
        private IOrderService _orderService;

        public FeedbackController(IFeedbackService feedbackService, IFeedbackDtoMapper feedbackDtoMapper, ISendEmailService sendEmailService, IOrderService orderService)
        {
            _feedbackService = feedbackService;
            _feedbackDtoMapper = feedbackDtoMapper;
            _sendEmailService = sendEmailService;
            _orderService = orderService;
        }
        [HttpGet]
        [Route("GetById/{id}")]
        public ApiResponse<Model.Dto.FeedBack> GetFeedbackByDeliveryId(string id)
        {
            try
            {
                var feedback = _feedbackService.GetFeedbackByDeliveryId(id);
                if (feedback != null)
                {
                    return ApiUtil<Model.Dto.FeedBack>.CreateSuccessfulResult(_feedbackDtoMapper.ToDto(feedback));
                }
                return ApiUtil<Model.Dto.FeedBack>.CreateSuccessfulResult(null);
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
        public async Task<ApiResponse> SendFeedbackEmail(string eventId)
        {
            try
            {
                var listUser = await _orderService.GetUserAlreadyOrderEmail(eventId);
                string path = System.Web.HttpContext.Current.Server.MapPath(Constant.FeedbackEmailTemplate);
                string emailTemplateJson = System.IO.File.ReadAllText(path);
                await _sendEmailService.SendEmailToAlreadyOrderedUserAsync(listUser, emailTemplateJson);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
