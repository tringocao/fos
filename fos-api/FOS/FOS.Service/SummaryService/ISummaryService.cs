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
        Task<string> AddReport(ReportFile report);
        void SendReport(string userEmail, string html);
        string BuildHtmlEmail(string reportUrl, string eventId, string reportId);
    }
}
