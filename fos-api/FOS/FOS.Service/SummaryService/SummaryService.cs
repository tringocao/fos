using FOS.Model.Domain;
using FOS.Services.SendEmailServices;
using FOS.Services.SPUserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.SummaryService
{
    public class SummaryService : ISummaryService
    {
        ISPUserService _spUserService;
        ISendEmailService _sendEmailService;
        public SummaryService(ISPUserService spUserService, ISendEmailService sendEmailService)
        {
            _spUserService = spUserService;
            _sendEmailService = sendEmailService;
        }

        public async Task SendEmailReportAsync()
        {
            //var smtpServer = site.WebApplication.OutboundMailServiceInstance.Parent.Name;
            User sender = await _spUserService.GetCurrentUser();
            MailMessage mail = new MailMessage(sender.UserPrincipalName, sender.UserPrincipalName, "report", "xxx");
            SmtpClient client = new SmtpClient("devpreciovn.sharepoint.com");
            client.Port = 587;
            client.EnableSsl = true;
            client.Host = "devpreciovn.sharepoint.com";
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(sender.UserPrincipalName, "!zyxzapr3");
            client.Send(mail);

            //_sendEmailService.SendEmailAsync
        }
    }
}
