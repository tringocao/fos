using AutoMapper;
using FOS.Model;
using FOS.Model.Domain;
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
    public class FOSFoodServiceAPIsRepository : RepositoryBase<FoodServiceAPI>, IFOSFoodServiceAPIsRepository
    {
        public FOSFoodServiceAPIsRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        //private AdventureWorks2014Entities DbContext2 = new AdventureWorks2014Entities();

        public IEnumerable<APIs> GetAllFOSCrawlLinks()
        {
            var list = DbContext.FoodServiceAPIs.Take(1).ToList();
            return Mapper.Map<IEnumerable<FoodServiceAPI>, IEnumerable<APIs>>(list);
        }

        public APIs GetFOSCrawlLinksById(int businessId)
        {
            var emp = DbContext.FoodServiceAPIs.Find(businessId);
            //var emp = new FOSCrawlLink() { id = 1, link = "https://gappapi.deliverynow.vn/api/delivery/get_infos" };
            return Mapper.Map<FoodServiceAPI, APIs>(emp);
        }
    }
}
