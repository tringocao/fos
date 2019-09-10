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
        IEnumerable<Model.Domain.Apis> GetAllFOSCrawlLinks();
        Model.Domain.Apis GetFOSCrawlLinksById(int businessId);
    }
}
