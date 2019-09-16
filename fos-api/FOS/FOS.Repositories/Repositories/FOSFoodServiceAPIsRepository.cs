using AutoMapper;
using FOS.Model;
using FOS.Repositories.DataModel;
using FOS.Repositories.Infrastructor;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public class FOSFoodServiceAPIsRepository : RepositoryBase<ExternalServiceAPI>, IFOSFoodServiceAPIsRepository
    {
        public FOSFoodServiceAPIsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        //private AdventureWorks2014Entities DbContext2 = new AdventureWorks2014Entities();

        public IEnumerable<Model.Domain.Apis> GetAllFOSCrawlLinks()
        {
            //throw new NotImplementedException();
            var list = DbContext.ExternalServiceAPIs;
            return Mapper.Map<IEnumerable<ExternalServiceAPI>, IEnumerable<Model.Domain.Apis>>(list);
        }

        public Model.Domain.Apis GetFOSCrawlLinksById(int businessId)
        {
            //throw new NotImplementedException();

            var emp = DbContext.ExternalServiceAPIs.Find(businessId);
            //var emp = new FOSCrawlLink() { id = 1, link = "https://gappapi.deliverynow.vn/api/delivery/get_infos" };
            return Mapper.Map<ExternalServiceAPI, Model.Domain.Apis>(emp);
        }
    }
}
