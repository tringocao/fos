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
using FOS.Model.Dto;
using FOS.Services.SPListService;
using FOS.Services.EventServices;
using System.Linq;
using FOS.Model.Domain;
using Newtonsoft.Json;

namespace FOS.Services.SummaryService
{
    public class SummaryService : ISummaryService
    {
        ISPUserService _spUserService;
        ISendEmailService _sendEmailService;
        IReportFileRepository _reportFileRepository;
        ISharepointContextProvider _sharepointContextProvider;
        IEventService _eventService;
        IOrderRepository _orderRepository;

        public SummaryService(ISPUserService spUserService, ISendEmailService sendEmailService, IReportFileRepository reportFileRepository, ISharepointContextProvider sharepointContextProvider, IEventService eventService, IOrderRepository orderRepository)
        {
            _spUserService = spUserService;
            _sendEmailService = sendEmailService;
            _reportFileRepository = reportFileRepository;
            _sharepointContextProvider = sharepointContextProvider;
            _eventService = eventService;
            _orderRepository = orderRepository;
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


        public void SendReport(string userEmail, string html, string subject = "Event Report")
        {
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                var emailp = new EmailProperties();
                string hostname = WebConfigurationManager.AppSettings[OAuth.HOME_URI];

                emailp.To = new List<string>() { userEmail };
                emailp.From = userEmail;
                emailp.Body = html;
                emailp.Subject = subject;

                Utility.SendEmail(clientContext, emailp);
                clientContext.ExecuteQuery();

            }
        }


        public async Task SendEmailReportAsync(Model.Dto.Report report)
        {
            byte[] fileBytes = Convert.FromBase64String(report.Attachment);

            Model.Domain.User sender = await _spUserService.GetCurrentUser();
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
        public IEnumerable<Model.Domain.RestaurantSummary> GetRestaurantSummary()
        {
            var allEvent = _eventService.GetAllEvent("");
            var allSummary = new List<Model.Domain.RestaurantSummary>();

            foreach(var item in allEvent){
                var summary = new Model.Domain.RestaurantSummary
                {
                    Restaurant = item.Restaurant,
                    RestaurantId = item.RestaurantId,
                    DeliveryId = item.DeliveryId,
                    ServiceId = item.ServiceId,
                    AppearTimes = 1
                };
                allSummary.Add(summary);
            }
            var uniqueSummary = RemoveDuplicateSummary(allSummary);
            var sortedSummary = uniqueSummary.OrderByDescending(s => s.AppearTimes).ToList();
            SetPercentAndRank(sortedSummary, allSummary.Count());

            return sortedSummary;
        }
        private IEnumerable<Model.Domain.RestaurantSummary> RemoveDuplicateSummary(IEnumerable<Model.Domain.RestaurantSummary> allSummary)
        {
            var uniqueSummary = allSummary.GroupBy(s => new { s.DeliveryId, s.RestaurantId, s.ServiceId }).ToList();
            var uniqueList = new List<Model.Domain.RestaurantSummary>();
            foreach (var element in uniqueSummary)
            {
                var unique = new Model.Domain.RestaurantSummary();
                unique = element.ToList()[0];
                unique.AppearTimes = element.ToList().Count();
                uniqueList.Add(unique);
            }
            return uniqueList;
        }
        private IEnumerable<Model.Domain.RestaurantSummary> SetPercentAndRank(List<Model.Domain.RestaurantSummary> sortedSummary, int numberOfEvent)
        {
            var maximumAppearTimes = sortedSummary.Max(s => s.AppearTimes);
            for(int i = 0 ; i < sortedSummary.Count(); i++)
            {
                sortedSummary[i].RelativePercent = ((float)sortedSummary[i].AppearTimes / (float)maximumAppearTimes) * (float)100;
                sortedSummary[i].Percent = ((float)sortedSummary[i].AppearTimes / (float)numberOfEvent) * (float)100;
                sortedSummary[i].Rank = i + 1;
            }
            return sortedSummary;
        }

        public IEnumerable<Model.Domain.DishesSummary> GetDishesSummary(string restaurantId, string deliveryId, string serviceId)
        {
            var allOrder = _orderRepository.GetOrdersOfSpecificRestaurant(restaurantId, deliveryId);
            var allSummary = new List<Model.Domain.DishesSummary>();

            foreach(var element in allOrder)
            {
                var summary = new Model.Domain.DishesSummary();
                summary.ApprearTimes = 1;
                var food = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, string>>>(element.FoodDetail); 
            }
            return null;
        }
    }
}
