using FOS.API.App_Start;
using FOS.Common;
using FOS.Model.Domain;
using FOS.Model.Util;
using FOS.Services;
using FOS.Services.Providers;
using FOS.Services.SPUserService;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/spuser")]
    public class SPUserController : ApiController
    {
        ISPUserService _sPUserService;

        public SPUserController(ISPUserService sPUserService)
        {
            _sPUserService = sPUserService;
        }

        // GET api/spuser/getusers
        [HttpGet]
        [Route("GetUsers")]
        public async Task<ApiResponse<List<Model.Dto.User>>> GetUsers()
        {
            try
            {
                var users = await _sPUserService.GetUsers();
                return ApiUtil<List<Model.Dto.User>>.CreateSuccessfulResult(users);
            }
            catch (Exception e)
            {
                return ApiUtil<List<Model.Dto.User>>.CreateFailResult(e.ToString());
            }
        }

        // GET api/spuser/GetCurrentUser
        [HttpGet]
        [Route("GetCurrentUserGraph")]
        public async Task<ApiResponse<Model.Dto.GraphUser>> GetCurrentUserGraph()
        {
            try
            {
                var user = await _sPUserService.GetCurrentUserGraph();
                return ApiUtil<Model.Dto.GraphUser>.CreateSuccessfulResult(user);
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.GraphUser>.CreateFailResult(e.ToString());
            }
        }

        [HttpGet]
        [Route("GetCurrentUser")]
        public async Task<ApiResponse<Model.Dto.User>> GetCurrentUser()
        {
            try
            {
                var user = await _sPUserService.GetCurrentUser();
                return ApiUtil<Model.Dto.User>.CreateSuccessfulResult(user);
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.User>.CreateFailResult(e.ToString());
            }
        }

        // GET api/spuser/GetUserById/Id
        [HttpGet]
        [Route("GetUserById")]
        public async Task<ApiResponse<Model.Domain.User>> GetUserById(string Id)
        {
            try
            {
                var user = await _sPUserService.GetUserById(Id);
                return ApiUtil<Model.Domain.User>.CreateSuccessfulResult(user);
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Domain.User>.CreateFailResult(e.ToString());
            }
        }

        // GET api/spuser/GetAvatarById/Id
        [HttpGet]
        [Route("GetGroups")]
        public async Task<ApiResponse<List<Model.Dto.User>>> GetGroups()
        {
            try
            {
                var group = await _sPUserService.GetGroups();
                return ApiUtil<List<Model.Dto.User>>.CreateSuccessfulResult(group);
            }
            catch (Exception e)
            {
                return ApiUtil<List<Model.Dto.User>>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetAvatar")]
        public async Task<HttpResponseMessage> GetAvatar(string Id, string avatarName)
        {
            var result = new HttpResponseMessage();
            try
            {
                var avatar = await _sPUserService.GetAvatar(Id, avatarName);
                
                result.Content = new ByteArrayContent(avatar);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                result.Headers.CacheControl = new CacheControlHeaderValue { Public = true, MaxAge = TimeSpan.FromDays(1) };

                return result;
            }
            catch (Exception e)
            {
                result.StatusCode = HttpStatusCode.NotFound;
                return result;
            }
        }

        [HttpGet]
        [Route("GetUsersByName")]
        public async Task<ApiResponse<List<Model.Dto.User>>> GetUsersByName(string searchName)
        {
            try
            {
                var user = await _sPUserService.GetUsersByName(searchName);
                return ApiUtil<List<Model.Dto.User>>.CreateSuccessfulResult(user);
            }
            catch (Exception e)
            {
                return ApiUtil<List<Model.Dto.User>>.CreateFailResult(e.ToString());
            }
        }

        [HttpGet]
        [Route("GroupListMemers")]
        public async Task<ApiResponse<List<Model.Dto.User>>> GroupListMemers(string groupId)
        {
            try
            {
                var user = await _sPUserService.GroupListMemers(groupId);
                return ApiUtil<List<Model.Dto.User>>.CreateSuccessfulResult(user);
            }
            catch (Exception e)
            {
                return ApiUtil<List<Model.Dto.User>>.CreateFailResult(e.ToString());
            }
        }
    }
}
