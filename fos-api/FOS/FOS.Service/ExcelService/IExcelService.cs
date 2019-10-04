using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.ExcelService
{
    public interface IExcelService
    {
        Task<bool> ExportCSV(ExcelModel excelModel);
    }
}
