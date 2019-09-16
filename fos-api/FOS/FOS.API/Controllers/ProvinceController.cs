using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.ExternalServices;
using FOS.Services.ProvinceServices;
using Newtonsoft.Json;
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
    [RoutePrefix("api/Province")]

    public class ProvinceController : ApiController
    {
        IProvinceService _provinceService;
        IProvinceDtoMapper _provinceDtoMapper;
        public ProvinceController(IProvinceService provinceService, IProvinceDtoMapper provinceDtoMapper)
        {
            _provinceService = provinceService;
        }
        // GET: api/Province 
        [HttpGet]
        [Route("GetAllProvince")]
        public async Task<ApiResponse<List<Province>>> GetAllProvince(int idService)
        {
            try
            {
                _provinceService.GetExternalServiceById(idService);
                var list = await _provinceService.GetMetadataForProvinceAsync();
                return ApiUtil<List<Province>>.CreateSuccessfulResult(
                    list.Select(p => _provinceDtoMapper.ToDto(p)).ToList()
                );
            }
            catch (Exception e)
            {
                return ApiUtil<List<Province>>.CreateFailResult(e.ToString());
            }
        }

        // GET: api/Province/5
        [HttpGet]
        [Route("GetById")]
        public async Task<ApiResponse<Province>> GetByIdAsync(int idService, int id)
        {
            try
            {
                _provinceService.GetExternalServiceById(idService);
                return ApiUtil<Province>.CreateSuccessfulResult(
                    _provinceDtoMapper.ToDto(await _provinceService.GetMetadataByIdAsync(id))
                );
            }
            catch (Exception e)
            {
                return ApiUtil<Province>.CreateFailResult(e.ToString());
            }
        }

        // POST: api/Province
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Province/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Province/5
        public void Delete(int id)
        {
        }
    }
}
