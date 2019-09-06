using FOS.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public interface IFOSFoodServiceAPIsRepository
    {
        IEnumerable<Apis> GetAllFOSCrawlLinks();
        Apis GetFOSCrawlLinksById(int businessId);
    }
}
