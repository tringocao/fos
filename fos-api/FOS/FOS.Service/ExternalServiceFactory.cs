using FOS.Model.Domain;
using FOS.Services.RequestMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services
{
    public class ExternalServiceFactory: IExternalServiceFactory
    {
        private ICrawlLinksService _crawlLinksSer;
        private IHostLinkService _hostLinkSer;
        private IEnumerable<IRequestMethod> _requestMethod { get; }

        //private IRequestMethod _requestMethod;s
        public ExternalServiceFactory(ICrawlLinksService crawlLinksSer, IHostLinkService hostLinkSer, IEnumerable<IRequestMethod> requestMethod)
        {
            this._crawlLinksSer = crawlLinksSer;
            this._hostLinkSer = hostLinkSer;
            this._requestMethod = requestMethod;
        }
        public Task<string> API(int id)
        {
            var api = _crawlLinksSer.GetById(id);
            return RunAPI(api);
        }
        private Task<string> RunAPI(APIs api)
        {
            var method = GetMethod(api);
            return method.GetResultAsync();
        }
        private IRequestMethod GetMethod(APIs api)
        {
            switch (api.Request_Method)
            {
                case "Post":
                    {
                        var temp = _requestMethod.Where(
                               x => x.GetType().Name.Equals("PostMethod")).FirstOrDefault();
                        temp.setAPI(api);
                        return temp;
                    }/*d*/
                case "Get":
                    {
                        var temp = _requestMethod.Where(
                               x => x.GetType().Name.Equals("GetMethod")).FirstOrDefault();
                        temp.setAPI(api);
                        return temp;
                    }
                default:
                    return null;
            }
        }


    }
}
