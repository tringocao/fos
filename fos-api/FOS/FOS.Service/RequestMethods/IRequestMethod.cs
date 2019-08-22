using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.RequestMethods
{
    public interface IRequestMethod
    {
        Task<string> GetResultAsync();
        void setAPI(APIs api);
    }
}
