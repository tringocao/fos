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

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/splist")]

    public class SPListController : ApiController
    {
        ISPListService _spListService;
        IEventService _eventService;
        private readonly IEventDtoMapper _eventDtoMapper;


        public SPListController(ISPListService spListService, IEventService eventService, IEventDtoMapper mapper)
        {
            _eventDtoMapper = mapper;
            _spListService = spListService;
            _eventService = eventService;
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
                var itemModel = _eventDtoMapper.ToModel(item);
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
                var result = _eventService.GetAllEvent(userId);
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
                var result = _eventService.GetEvent(id);
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
                var domainItem = _eventDtoMapper.ToModel(item);
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
                await _spListService.UpdateEventStatus(id, eventStatus);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
