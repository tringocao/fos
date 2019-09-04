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
        [Route("getusers")]
        public async Task<ApiResponse<string>> GetUsers()
        {
            try
            {
                var users = await _sPUserService.GetUsers();
                return ApiUtil<string>.CreateSuccessfulResult(users);
            }
            catch (Exception e)
            {
                return ApiUtil<string>.CreateFailResult(e.ToString());
            }
        }

        // GET api/spuser/GetCurrentUser
        [HttpGet]
        [Route("GetCurrentUser")]
        public async Task<ApiResponse<Model.Domain.User>> GetCurrentUser()
        {
            try
            {
                var user = await _sPUserService.GetCurrentUser();
                return ApiUtil<Model.Domain.User>.CreateSuccessfulResult(user);
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Domain.User>.CreateFailResult(e.ToString());
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
        public async Task<ApiResponse<string>> GetGroups()
        {
            try
            {
                var group = await _sPUserService.GetGroups();
                return ApiUtil<string>.CreateSuccessfulResult(group);
            }
            catch (Exception e)
            {
                return ApiUtil<string>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetAvatarByUserId")]
        public async Task<ApiResponse<byte[]>> GetAvatarByUserId(string Id)
        {
            try
            {
                var avatar = await _sPUserService.GetAvatarByUserId(Id);
                return ApiUtil<byte[]>.CreateSuccessfulResult(avatar);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        //public async Task<HttpResponseMessage> GetContext()
        //{
        //    using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
        //    {
        //        var web = clientContext.Web;
        //        clientContext.Load(web);
        //        clientContext.ExecuteQuery();
        //        if (clientContext.Web.IsPropertyAvailable("Title"))
        //        {
        //            Console.WriteLine("Found title");
        //        }
        //        Console.WriteLine("Title: {0}", web.Title);
        //    }
        //    HttpResponseMessage responde = new HttpResponseMessage();

        //    return responde;
        //}
    }
}
