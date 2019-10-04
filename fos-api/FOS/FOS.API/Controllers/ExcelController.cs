using FOS.API.App_Start;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services.CustomGroupService;
using FOS.Services.ExcelService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using System.Text;
using FOS.Model.Dto;
using FOS.Model.Domain;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/Excel")]
    public class ExcelController : ApiController
    {
        IExcelService _excelService;
        IUserOrderDtoMapper _userOrderDtoMapper;
        IExcelModelDtoMapper _excelMapper;

        public ExcelController(IUserOrderDtoMapper userOrderDtoMapper, IExcelService excelService, IExcelModelDtoMapper excelMapper)
        {
            _userOrderDtoMapper = userOrderDtoMapper;
            _excelService = excelService;
            _excelMapper = excelMapper;
        }
        [HttpPost]
        [Route("CreateCSV")]
        public async Task<ApiResponse> CreateGroup([FromBody]Model.Dto.ExcelModel excelModel)
        {
            try
            {
                Model.Domain.ExcelModel domainExcelModel = _excelMapper.ToDomain(excelModel);
                await _excelService.ExportCSV(domainExcelModel);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("DownloadCSV")]
        public HttpResponseMessage DownloadCSV()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {               
                response.Content = new StreamContent(new FileStream(Common.Constants.Constant.FileXlsxDirectory, FileMode.Open, FileAccess.Read));
                //response.Content.Headers.ContentType.CharSet = Encoding.UTF8.HeaderName;
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = Common.Constants.Constant.FileXlsxNameWithExtension;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                return response;
            }
        }
    }
}
