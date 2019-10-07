using FOS.API.App_Start;
using FOS.Common;
using FOS.Model.Domain;
using FOS.Model.Util;
using FOS.Services;
using FOS.Services.Providers;
using FOS.Services.EventServices;
using FOS.Services.SPListService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Services.SPUserService;
using FOS.Common.Constants;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/splist")]

    public class SPListController : ApiController
    {
        ISPListService _spListService;
        IEventService _eventService;
        private readonly IEventDtoMapper _eventDtoMapper;
        ISPUserService _userService;

        public SPListController(ISPListService spListService, IEventService eventService, IEventDtoMapper mapper, ISPUserService userService)
        {
            _eventDtoMapper = mapper;
            _spListService = spListService;
            _eventService = eventService;
            _userService = userService;
        }
        //// GET api/splist/getlist/{list-id}
        //public async Task<HttpResponseMessage> GetList(string Id)
        //{
        //    return await _spListService.SendAsync(HttpMethod.Get, "sites/lists/" + Id, null);
        //}

        // POST api/splist/addlistitem/{list-id}/
        [HttpPost]
        [Route("AddListItem")]
        public async Task<ApiResponse> AddListItem(string Id, [FromBody]JsonRequest item)
        {
            try
            {
                await _spListService.AddListItem(Id, item);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }

        [HttpPost]
        [Route("AddEventListItem")]
        public ApiResponse<string> AddEventListItem(string id, [FromBody]Model.Dto.Event item)
        {
            try
            {
                var itemModel = _eventDtoMapper.DtoToDomain(item);
                bool check = _eventService.ValidateEventInfo(itemModel);
                if (check == false)
                {
                    return ApiUtil<string>.CreateFailResult(Constant.NotValidEventInfo);
                }

                var result = _spListService.AddEventListItem(id, itemModel);
                return ApiUtil<string>.CreateSuccessfulResult(result);
            }
            catch (Exception e)
            {
                return ApiUtil<string>.CreateFailResult(e.ToString());
            }
        }
        public ApiResponse<IEnumerable<Model.Dto.Event>> GetAllEvent(string userId)
        {
            try
            {
                var allEvent = _eventService.GetAllEvent(userId);
                var result = _eventDtoMapper.ListDomainToDto(allEvent);
                return ApiUtil<IEnumerable<Model.Dto.Event>>.CreateSuccessfulResult(result);
            }
            catch (Exception e)
            {
                return ApiUtil<IEnumerable<Model.Dto.Event>>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetEventById")]
        public ApiResponse<Model.Dto.Event> GetEvent(int id)
        {
            try
            {
                var element = _eventService.GetEvent(id);
                var result = _eventDtoMapper.DomainToDto(element);
                return ApiUtil<Model.Dto.Event>.CreateSuccessfulResult(result);
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.Event>.CreateFailResult(e.ToString());
            }
        }

        [HttpPost]
        [Route("UpdateListItem")]
        public async Task<ApiResponse> UpdateListItem(string id, [FromBody]Model.Dto.Event item)
        {
            try
            {
                
                //bool checkHost = await _userService.ValidateIsHost(Int32.Parse(id));
                //if (checkHost == false)
                //{
                //    return ApiUtil.CreateFailResult(Constant.UserNotPerission);
                //}

                var domainItem = _eventDtoMapper.DtoToDomain(item);
                //bool check = _eventService.ValidateEventInfo(domainItem);
                //if (check == false)
                //{
                //    return ApiUtil<string>.CreateFailResult(Constant.NotValidEventInfo);
                //}
                await _spListService.UpdateListItem(id, domainItem);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }

        [HttpPost]
        [Route("UpdateEventStatus")]
        public async Task<ApiResponse> UpdateEventStatus(string id, string eventStatus)
        {
            try
            {
                bool checkHost = await _userService.ValidateIsHost(Int32.Parse(id));
                if (checkHost == false)
                {
                    return ApiUtil.CreateFailResult(Constant.UserNotPerission);
                }
                if(eventStatus == EventStatus.Reopened)
                {
                   Model.Domain.Event currEvent =   _eventService.GetEvent(Int32.Parse(id));
                    if(currEvent.Status != EventStatus.Closed)
                    {
                        return ApiUtil.CreateFailResult(Constant.NotValidEventInfo);
                    }
                }
                if (eventStatus == EventStatus.Error)
                {
                    Model.Domain.Event currEvent = _eventService.GetEvent(Int32.Parse(id));
                    if (currEvent.Status != EventStatus.Opened)
                    {
                        return ApiUtil.CreateFailResult(Constant.NotValidEventInfo);
                    }
                }
                await _spListService.UpdateEventStatus(id, eventStatus);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpPut]
        [Route("SetTime2Close")]
        public async Task<ApiResponse> SetTime2Close(string id,[FromBody]DateTime dateTime)
        {
            try
            {
                await _spListService.SetTime2Close(id, dateTime);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpPost]
        [Route("UpdateListItemWhenRestaurantChanges")]
        public async Task<ApiResponse> UpdateListItemWhenRestaurantChanges(string id, [FromBody]Model.Dto.Event item)
        {
            try
            {
                var domainItem = _eventDtoMapper.DtoToDomain(item);
                bool checkHost = await _userService.ValidateIsHost(Int32.Parse(id));
                if (checkHost == false)
                {
                    return ApiUtil.CreateFailResult(Constant.UserNotPerission);
                }

                bool check = _eventService.ValidateEventInfo(domainItem);
                if (check == false)
                {
                    return ApiUtil<string>.CreateFailResult(Constant.NotValidEventInfo);
                }
                await _spListService.UpdateListItemWhenRestaurantChanges(id, domainItem);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
