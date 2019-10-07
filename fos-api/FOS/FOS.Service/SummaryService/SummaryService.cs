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
using FOS.Common.Constants;
using FOS.Repositories.Mapping;

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
        IFeedbackRepository _feedBackRepository;
        IFeedbackMapper _feedbackMapper;

        public SummaryService(ISPUserService spUserService, ISendEmailService sendEmailService, IReportFileRepository reportFileRepository, ISharepointContextProvider sharepointContextProvider, IEventService eventService, IOrderRepository orderRepository, IFeedbackRepository feedbackRepository, IFeedbackMapper feedbackMapper)
        {
            _spUserService = spUserService;
            _sendEmailService = sendEmailService;
            _reportFileRepository = reportFileRepository;
            _sharepointContextProvider = sharepointContextProvider;
            _eventService = eventService;
            _orderRepository = orderRepository;
            _feedBackRepository = feedbackRepository;
            _feedbackMapper = feedbackMapper;
        }

        public string GetReportContentByEventId(string eventId)
        {
            return _reportFileRepository.GetOne(eventId).Content;
        }

        public string AddReport(ReportFile report)
        {
            Guid reportId = Guid.NewGuid();
            report.Name = reportId.ToString();
            string rawContent = report.Content.Substring(report.Content.IndexOf(",") + 1);
            report.Content = rawContent;
            _reportFileRepository.AddReport(report);
            return reportId.ToString();
        }

        public async Task<ReportEmailTemplate> BuildEmailTemplate(string reportUrl, string eventId, string reportId)
        {
            var imageUrl = WebConfigurationManager.AppSettings[OAuth.WEBAPI_HOME_URI] + "api/summary/GetImage/" + reportId;
            var currentUser = await _spUserService.GetCurrentUser(); 
            var reportEmailTemplate = new ReportEmailTemplate();
            var eventIdIntValue = Int32.Parse(eventId);
            reportEmailTemplate.EventTitle = _eventService.GetEvent(eventIdIntValue).Name;
            reportEmailTemplate.ImageUrl = imageUrl;
            reportEmailTemplate.LinkToEventReport = reportUrl;
            reportEmailTemplate.UserName = currentUser.DisplayName;

            return reportEmailTemplate;
        }


        public async Task SendReportAsync(string eventId, string reportUrl, ReportFile report)
        {
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                string path = System.Web.HttpContext.Current.Server.MapPath(Constant.ReportEmailTemplate);
                string emailTemplateJson = System.IO.File.ReadAllText(path);
                var jsonTemplate = JsonConvert.DeserializeObject<Dictionary<string, string>>(emailTemplateJson);
                jsonTemplate.TryGetValue("Body", out string body);
                jsonTemplate.TryGetValue("Subject", out string subject);

                Model.Domain.User sender = await _spUserService.GetCurrentUser();
                string reportId = AddReport(report);
                var emailTemplate = await BuildEmailTemplate(reportUrl, eventId, reportId);

                subject = _sendEmailService.Parse(subject, emailTemplate);
                body = _sendEmailService.Parse(body, emailTemplate);

                var emailp = new EmailProperties();
                string hostname = WebConfigurationManager.AppSettings[OAuth.HOME_URI];

                emailp.To = new List<string>() { sender.Mail };
                emailp.From = sender.Mail;
                emailp.Body = body;
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
            var allEventWithoutError = allEvent.Where(e => e.Status != EventStatus.Error).ToList();
            var allSummary = new List<Model.Domain.RestaurantSummary>();

            foreach(var item in allEventWithoutError)
            {
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
            if(allSummary.Count > 0)
            {
                var uniqueSummary = RemoveDuplicateRestaurantSummary(allSummary);
                var sortedSummary = uniqueSummary.OrderByDescending(s => s.AppearTimes).ToList();
                SetPercentAndRankRestaurant(sortedSummary, allSummary.Count());

                return sortedSummary;
            }
            return null;
        }
        private IEnumerable<Model.Domain.RestaurantSummary> RemoveDuplicateRestaurantSummary(IEnumerable<Model.Domain.RestaurantSummary> allSummary)
        {
            var uniqueSummary = allSummary.GroupBy(s => new { s.DeliveryId, s.RestaurantId, s.ServiceId }).ToList();
            var uniqueList = new List<Model.Domain.RestaurantSummary>();
            foreach (var element in uniqueSummary)
            {
                var unique = new Model.Domain.RestaurantSummary();
                unique = element.ToList()[0];
                unique.AppearTimes = element.ToList().Count();

                var feedback = _feedBackRepository.GetById(unique.DeliveryId);
                if (feedback != null)
                {
                    var domainFeedback = _feedbackMapper.MapToDomain(feedback);
                    unique.AverageRating = domainFeedback.Ratings.Sum(item => item.Value) / (float)domainFeedback.Ratings.Count;
                }
                uniqueList.Add(unique);
            }
            return uniqueList;
        }
        private IEnumerable<Model.Domain.RestaurantSummary> SetPercentAndRankRestaurant(List<Model.Domain.RestaurantSummary> sortedSummary, int numberOfEvent)
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
            var allOrderWithoutError = GetAllOrderWithoutErrorEvent(allOrder);
            var allSummary = new List<Model.Domain.DishesSummary>();

            foreach(var element in allOrderWithoutError)
            {
                var food = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, string>>>(element.FoodDetail);
                if(food != null && food.Count > 0)
                {
                    var foodIds = food.Keys;
                    foreach (var id in foodIds)
                    {
                        var summary = new Model.Domain.DishesSummary();
                        summary.AppearTimes = 1;
                        summary.FoodId = id;
                        summary.Food = food[id]["Name"];
                        allSummary.Add(summary);
                    }
                }
            }
            if(allSummary.Count > 0)
            {
                var uniqueSummary = RemoveDuplicateDishesSummary(allSummary);
                var sortedSummary = uniqueSummary.OrderByDescending(s => s.AppearTimes).ToList();
                SetPercentAndRankDishes(sortedSummary, allSummary.Count());

                return sortedSummary;
            }
            return null;
        }
        private IEnumerable<Model.Domain.DishesSummary> RemoveDuplicateDishesSummary(IEnumerable<Model.Domain.DishesSummary> allSummary)
        {
            var uniqueSummary = allSummary.GroupBy(s => s.FoodId).ToList();
            var uniqueList = new List<Model.Domain.DishesSummary>();
            foreach (var element in uniqueSummary)
            {
                var unique = new Model.Domain.DishesSummary();
                unique = element.ToList()[0];
                unique.AppearTimes = element.ToList().Count();
                uniqueList.Add(unique);
            }
            return uniqueList;
        }
        private IEnumerable<Model.Domain.DishesSummary> SetPercentAndRankDishes(List<Model.Domain.DishesSummary> sortedSummary, int numberOfEvent)
        {
            var maximumAppearTimes = sortedSummary.Max(s => s.AppearTimes);
            for (int i = 0; i < sortedSummary.Count(); i++)
            {
                sortedSummary[i].RelativePercent = ((float)sortedSummary[i].AppearTimes / (float)maximumAppearTimes) * (float)100;
                sortedSummary[i].Percent = ((float)sortedSummary[i].AppearTimes / (float)numberOfEvent) * (float)100;
                sortedSummary[i].Rank = i + 1;
            }
            return sortedSummary;
        }
        private IEnumerable<Repositories.DataModel.Order> GetAllOrderWithoutErrorEvent(IEnumerable<Repositories.DataModel.Order> orders)
        {
            if(orders.Count() > 0)
            {
                var allEvent = _eventService.GetAllEvent("");
                var errorEvent = allEvent.Where(e => e.Status == EventStatus.Error).Select(e => e.EventId).ToList();

                return orders.Where(o => !errorEvent.Contains(o.IdEvent)).ToList();
            }
            return new List<Repositories.DataModel.Order>();
        }
    }
}
