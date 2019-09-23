using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.RecurrenceEventServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/RecurrenceEvent")]
    public class RecurrenceEventController : ApiController
    {
        IRecurrenceEventService _recurrenceEventService;
        IRecurrenceEventDtoMapper _recurrenceEventDtoMapper;
        public RecurrenceEventController(IRecurrenceEventService recurrenceEventService, IRecurrenceEventDtoMapper recurrenceEventDtoMapper)
        {
            _recurrenceEventService = recurrenceEventService;
            _recurrenceEventDtoMapper = recurrenceEventDtoMapper;
        }
        [HttpGet]
        [Route("GetById")]
        public ApiResponse<Model.Dto.RecurrenceEvent> GetById(int recId)
        {
            try
            {
                return ApiUtil<Model.Dto.RecurrenceEvent>.CreateSuccessfulResult(
                    _recurrenceEventDtoMapper.ToDto(_recurrenceEventService.GetById(recId))
               );
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.RecurrenceEvent>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetByUserId")]
        public ApiResponse<Model.Dto.RecurrenceEvent> GetByUserId(string userId)
        {
            try
            {
                return ApiUtil<Model.Dto.RecurrenceEvent>.CreateSuccessfulResult(
                    _recurrenceEventDtoMapper.ToDto(_recurrenceEventService.GetByUserId(userId))
               );
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.RecurrenceEvent>.CreateFailResult(e.ToString());
            }
        }
        [HttpPost]
        [Route("AddNew")]
        public ApiResponse AddNew([FromBody]Model.Dto.RecurrenceEvent recurrenceEvent)
        {
            try
            {
                recurrenceEvent.StartTempDate = recurrenceEvent.StartDate.ToLocalTime();
                _recurrenceEventService.AddRecurrenceEvent(_recurrenceEventDtoMapper.ToModel(recurrenceEvent));
                if (recurrenceEvent.StartTempDate >= DateTime.Now
                    && recurrenceEvent.StartTempDate < DateTime.Now.AddHours(1))
                {
                    _recurrenceEventService.RunThisTask(_recurrenceEventDtoMapper.ToModel(recurrenceEvent));
                }
                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpPost]
        [Route("UpdateRecurrenceEvent")]
        public ApiResponse UpdateRecurrenceEvent([FromBody]Model.Dto.RecurrenceEvent recurrenceEvent)
        {
            try
            {
                Model.Domain.RecurrenceEvent before = _recurrenceEventService.GetById(recurrenceEvent.Id);
                recurrenceEvent.StartTempDate = recurrenceEvent.StartDate.ToLocalTime();
                recurrenceEvent.Version += 1;
                _recurrenceEventService.UpdateRecurrenceEvent(_recurrenceEventDtoMapper.ToModel(recurrenceEvent));

                if (recurrenceEvent.StartTempDate >= DateTime.Now
                    && recurrenceEvent.StartTempDate < DateTime.Now.AddHours(1))
                {
                    _recurrenceEventService.RunThisTask(_recurrenceEventDtoMapper.ToModel(recurrenceEvent));
                }
                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }

        // POST: api/RecurrenceEvent
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/RecurrenceEvent/5
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpGet]
        [Route("DeleteById")]
        public ApiResponse Delete(int id)
        {
            try
            {
                if (id == 0) return ApiUtil.CreateSuccessfulResult();

                _recurrenceEventService.DeleteById(id);
                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
