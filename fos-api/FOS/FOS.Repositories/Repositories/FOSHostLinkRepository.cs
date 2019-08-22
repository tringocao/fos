using AutoMapper;
using FOS.Model.Domain;
using FOS.Repositories.Infrastructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public class FOSHostLinkRepository : RepositoryBase<FOSCrawlLink>, IFOSHostLinkRepository
    {

        public FOSHostLinkRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        public IEnumerable<Host> GetAllHosts()
        {
            var list = DbContext.FOSHostLinks.Take(1).ToList();
            return Mapper.Map<IEnumerable<FOSHostLink>, IEnumerable<Host>>(list);
        }
        public Host GetHostById(int Id)
        {
            var emp = DbContext.FOSHostLinks.Find(Id);
            //var emp = new FOSCrawlLink() { id = 1, link = "https://gappapi.deliverynow.vn/api/delivery/get_infos" };
            return Mapper.Map<FOSHostLink, Host>(emp);
        }
    }
}
