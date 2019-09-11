using FOS.Services.SendEmailServices;
using FOS.Services.SPUserService;
using System;
using System.IO;
using System.Threading.Tasks;
using SendGrid;
using FOS.Repositories.Repositories;
using FOS.Repositories.DataModel;
using Microsoft.SharePoint.Client;
using FOS.Services.Providers;
using FOS.Common;
using Microsoft.SharePoint.Client.Utilities;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FOS.Services.SummaryService
{
    public class SummaryService : ISummaryService
    {
        ISPUserService _spUserService;
        ISendEmailService _sendEmailService;
        IReportFileRepository _reportFileRepository;
        ISharepointContextProvider _sharepointContextProvider;

        public SummaryService(ISPUserService spUserService, ISendEmailService sendEmailService, IReportFileRepository reportFileRepository, ISharepointContextProvider sharepointContextProvider)
        {
            _spUserService = spUserService;
            _sendEmailService = sendEmailService;
            _reportFileRepository = reportFileRepository;
            _sharepointContextProvider = sharepointContextProvider;
        }

        public string GetReportContentByEventId(string eventId)
        {
            return _reportFileRepository.GetOne(eventId).Content;
        }

        public async Task<string> AddReport(ReportFile report)
        {
            Guid reportId = Guid.NewGuid();
            report.Name = reportId.ToString();
            string rawContent = report.Content.Substring(report.Content.IndexOf(",") + 1);
            report.Content = rawContent;
            _reportFileRepository.AddReport(report);
            return reportId.ToString();
        }

        public string BuildHtmlEmail(string reportUrl, string eventId, string reportId)
        {
            var imageUrl = WebConfigurationManager.AppSettings[OAuth.WEBAPI_HOME_URI] + "api/summary/GetImage/" + reportId;
            var html = "<html> <a href='" + reportUrl + "'>Click here to go to event report" + "</a>" +
                "</a></br><img data-imagetype='External' style='width: 500px; height: 500px;' src='" + imageUrl + "' />"+
                "</br>Click <a href='" + imageUrl + "' target='_blank'>here </a> if you couldn't see the image" +
            "</html>";
            return html;
        }


        public void SendReport(string userEmail, string html)
        {
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                var emailp = new EmailProperties();
                string hostname = WebConfigurationManager.AppSettings[OAuth.HOME_URI];

                emailp.To = new List<string>() { userEmail };
                emailp.From = userEmail;
                emailp.Body = html;
                emailp.Subject = "Event Report";

                Utility.SendEmail(clientContext, emailp);
                clientContext.ExecuteQuery();

            }
        }


        public async Task SendEmailReportAsync(Model.Dto.Report report)
        {
            byte[] fileBytes = Convert.FromBase64String(report.Attachment);

            Model.Dto.User sender = await _spUserService.GetCurrentUser();
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY", EnvironmentVariableTarget.User);
            var client = new SendGridClient(apiKey);
            var from = new SendGrid.Helpers.Mail.EmailAddress(sender.Mail, sender.UserPrincipalName);
            var subject = report.Subject;
            var to = new SendGrid.Helpers.Mail.EmailAddress(sender.Mail, sender.UserPrincipalName);
            var plainTextContent = "";
            var htmlContent = report.Html;
            var msg = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            //var pdfBinary = Convert.FromBase64String(report.Attachment);
            msg.AddAttachment("Report.pdf", report.Attachment);
            //msg.AddAttachment()

            var response = await client.SendEmailAsync(msg);

            var result = response.Body.ReadAsStringAsync().Result;
            //var smtpServer = site.WebApplication.OutboundMailServiceInstance.Parent.Name;
            //User sender = await _spUserService.GetCurrentUser();
            //MailMessage mail = new MailMessage(sender.UserPrincipalName, sender.UserPrincipalName, "report", "xxx");
            //SmtpClient client = new SmtpClient("devpreciovn.sharepoint.com");
            //client.Port = 587;
            //client.EnableSsl = true;
            //client.Host = "devpreciovn.sharepoint.com";
            //client.Timeout = 10000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.Credentials = new System.Net.NetworkCredential(sender.UserPrincipalName, "!zyxzapr3");
            //client.Send(mail);

            //_sendEmailService.SendEmailAsync
        }
    }
}
