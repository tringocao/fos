
using FOS.Model.Domain;
using FOS.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services
{
    public class HostLinkService: IHostLinkService
    {
        private IFOSHostLinkRepository _crawlHostRepo;

        public HostLinkService(IFOSHostLinkRepository crawlHostRepo)
        {
            this._crawlHostRepo = crawlHostRepo;
        }
        public IEnumerable<Host> GetAllHosts()
        {
            return _crawlHostRepo.GetAllHosts();
        }
        public Host GetById(int Id)
        {
            return _crawlHostRepo.GetHostById(Id);
        }

    }
}
