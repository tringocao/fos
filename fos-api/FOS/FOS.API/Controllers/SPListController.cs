using FOS.API.App_Start;
using FOS.Common;
using FOS.Model.Domain;
using FOS.Model.Util;
using FOS.Services;
using FOS.Services.Providers;
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

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/splist")]

    public class SPListController : ApiController
    {
        ISPListService _spListService;
        public SPListController(ISPListService spListService)
        {
            _spListService = spListService;
        }
        //// GET api/splist/getlist/{list-id}
        //public async Task<HttpResponseMessage> GetList(string Id)
        //{
        //    return await _spListService.SendAsync(HttpMethod.Get, "sites/lists/" + Id, null);
        //}

        // POST api/splist/addlistitem/{list-id}/
        [HttpPost]
        [Route("AddListItem")]
        public async Task<ApiResponse> AddListItem(string Id, [FromBody]JSONRequest item)
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
        public ApiResponse AddEventListItem(string Id, [FromBody]EventList item)
        {
            try
            {
                _spListService.AddEventListItem(Id, item);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
