using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.SummaryService
{
    public interface ISummaryService
    {
        Task SendEmailReportAsync();
    }
}
