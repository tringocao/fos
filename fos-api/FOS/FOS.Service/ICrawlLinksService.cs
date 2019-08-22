using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services
{
    public interface ICrawlLinksService
    {
        IEnumerable<APIs> GetAllFOSCrawlLinks();
        Task<string> GetByIdAsync(int businessId);
        APIs GetById(int Id);

    }
}
