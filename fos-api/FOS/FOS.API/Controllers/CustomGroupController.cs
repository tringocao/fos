using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.CustomGroupService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/CustomGroup")]
    public class CustomGroupController : ApiController
    {
        ICustomGroupService _customGroupService;
        ICustomGroupDtoMapper _customGroupDtoMapper;
        public CustomGroupController(ICustomGroupService customGroupService, ICustomGroupDtoMapper customGroupDtoMapper)
        {
            _customGroupService = customGroupService;
            _customGroupDtoMapper = customGroupDtoMapper;
        }
        [HttpPost]
        [Route("CreateGroup")]
        public ApiResponse CreateGroup([FromBody]Model.Dto.CustomGroup customGroup)
        {
            try
            {
                var domainGroup = _customGroupDtoMapper.DtoToDomain(customGroup);
                _customGroupService.CreateGroup(domainGroup);
                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetAllGroup")]
        public ApiResponse<List<Model.Dto.CustomGroup>> GetAllGroup(string ownerId)
        {
            try
            {
                var domainGroups = _customGroupService.GetAll(ownerId);
                var dtoGroups = domainGroups.Select(g => _customGroupDtoMapper.DomainToDto(g)).ToList();
                return ApiUtil<List<Model.Dto.CustomGroup>>.CreateSuccessfulResult(dtoGroups);

            }
            catch (Exception e)
            {
                return ApiUtil<List<Model.Dto.CustomGroup>>.CreateFailResult(e.ToString());
            }
        }
        [HttpPost]
        [Route("UpdateGroup")]
        public ApiResponse UpdateGroup([FromBody]Model.Dto.CustomGroup customGroup)
        {
            try
            {
                var domainGroup = _customGroupDtoMapper.DtoToDomain(customGroup);
                _customGroupService.UpdateGroup(domainGroup);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpDelete]
        [Route("DeleteGroupById")]
        public ApiResponse DeleteGroupById(string groupId)
        {
            try
            {
                _customGroupService.DeleteGroupById(groupId);
                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
