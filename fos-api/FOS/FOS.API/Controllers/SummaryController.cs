using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Model.Util;
using FOS.Services.SendEmailServices;
using FOS.Services.SPUserService;
using FOS.Services.SummaryService;
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
    [RoutePrefix("api/summary")]
    public class SummaryController : ApiController
    {
        ISummaryService _summaryService;
        ISendEmailService _sendEmailService;
        ISPUserService _spUserService;

        public SummaryController(ISummaryService summaryService, ISendEmailService sendEmailService, ISPUserService spUserService)
        {
            _summaryService = summaryService;
            _sendEmailService = sendEmailService;
            _spUserService = spUserService;
        }

        // GET: api/favoriterestaurant/getall
        [HttpPost]
        [Route("SendReport")]
        public async Task<ApiResponse> SendReport([FromBody]Report report)
        {
            try
            {
                Model.Domain.User sender = await _spUserService.GetCurrentUser();
                _sendEmailService.SendReport(sender.UserPrincipalName, report.html);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
    }
}
