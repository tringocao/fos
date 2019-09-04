using FOS.Common;
using FOS.Services.Providers;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.SendEmailServices
{
    class SendEmailService: ISendEmailService
    {
    //    ISharepointContextProvider _sharepointContextProvider;
    //    ISPUserService _sPUserService;

    //    public SendEmailService(ISharepointContextProvider sharepointContextProvider, ISPUserService sPUserService)
    //    {
    //        _sharepointContextProvider = sharepointContextProvider;
    //        _sPUserService = sPUserService;
    //    }
    //    public async Task<ApiOperationResult<string>> SendEmail()
    //    {
    //        using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
    //        {
    //                var emailp = new EmailProperties();
    //                emailp.BCC = new List<string> { "a@mail.com" };
    //                emailp.To = new List<string> { "b@mail.com" };
    //                emailp.From = "from@mail.com";
    //                emailp.Body = "<b>html</b>";
    //                emailp.Subject = "subject";

    //                Utility.SendEmail(clientContext, emailp);
    //                clientContext.ExecuteQuery();
    //        }

    //        return ApiUtil<string>.CreateSuccessResult("Employee Published!");

    //    }
    }
}
