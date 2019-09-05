using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.Providers
{
    public interface IGraphApiProvider
    {
        Task<HttpResponseMessage> SendAsync(HttpMethod method, string endPoint, string data);
    }
}
