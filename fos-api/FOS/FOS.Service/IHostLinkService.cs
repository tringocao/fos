using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services
{
    public interface IHostLinkService
    {
        IEnumerable<Host> GetAllHosts();
        //string GetByIdAsync(int Id);
        Host GetById(int Id);
    }
}
