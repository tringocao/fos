using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services;
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
    [RoutePrefix("api/External")]

    public class ExternalServiceController : ApiController
    {
        IFOSFoodServiceAPIsService _iFOSFoodServiceAPIsService;
        IAPIsDtoMapper _iAPIsDtoMapper;
        public ExternalServiceController(IFOSFoodServiceAPIsService iFOSFoodServiceAPIsService, IAPIsDtoMapper iAPIsDtoMapper)
        {
            _iFOSFoodServiceAPIsService = iFOSFoodServiceAPIsService;
            _iAPIsDtoMapper = iAPIsDtoMapper;

        }
        [HttpGet]
        [Route("GetAllExternalService")]
        public async Task<ApiResponse<IEnumerable<ExternalService>>> GetAllExternalService()
        {
            try
            {
                var list = _iFOSFoodServiceAPIsService.GetAll();
                return ApiUtil<IEnumerable<ExternalService>>.CreateSuccessfulResult(
                    list.Select(p => _iAPIsDtoMapper.ToDto(p))
                );
            }
            catch (Exception e)
            {
                return ApiUtil<IEnumerable<ExternalService>>.CreateFailResult(e.ToString());
            }
        }
    }
}