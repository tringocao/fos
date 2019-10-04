using FOS.Model.Domain;
using FOS.Repositories.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.SummaryService
{
    public interface ISummaryService
    {
        Task SendEmailReportAsync(Model.Dto.Report report);
        string GetReportContentByEventId(string eventId);
        string AddReport(ReportFile report);
        Task SendReportAsync(string eventId, string reportUrl, ReportFile report);
        Task<ReportEmailTemplate> BuildEmailTemplate(string reportUrl, string eventId, string reportId);
        IEnumerable<RestaurantSummary> GetRestaurantSummary();
        IEnumerable<DishesSummary> GetDishesSummary(string restaurantId, string deliveryId, string serviceId);
    }
}
