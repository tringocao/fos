using FOS.Model.Domain;
using FOS.Services.RequestMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services
{
    public interface IExternalServiceFactory
    {
        Task<string> API(int id);
        //IRequestMethod GetMethod(APIs api);
        


    }
}
