using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public interface IFOSHostLinkRepository
    {
        IEnumerable<Host> GetAllHosts();
        Host GetHostById(int Id);
    }
}
