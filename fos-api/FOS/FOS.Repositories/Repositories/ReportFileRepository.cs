using AutoMapper;
using FOS.Repositories.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public interface IReportFileRepository
    {
        IEnumerable<ReportFile> GetAll();
        void AddReport(ReportFile report);
        ReportFile GetOne(string name);
    }
    public class ReportFileRepository : IReportFileRepository
    {
        private readonly FosContext _context;
        public ReportFileRepository(FosContext context)
        {
            _context = context;
        }

        public void AddReport(ReportFile report)
        {
            var _report = Mapper.Map<ReportFile, DataModel.ReportFile>(report);
            _context.ReportFiles.Add(_report);
            _context.SaveChanges();
        }

        public IEnumerable<ReportFile> GetAll()
        {
            var list = _context.ReportFiles.ToList();
            return Mapper.Map<IEnumerable<DataModel.ReportFile>, IEnumerable<ReportFile>>(list);
        }

        public ReportFile GetOne(string name)
        {
            var report = _context.ReportFiles.Where(_report => _report.Name == name).FirstOrDefault();
            return Mapper.Map<DataModel.ReportFile, ReportFile>(report);
        }
    }
}
