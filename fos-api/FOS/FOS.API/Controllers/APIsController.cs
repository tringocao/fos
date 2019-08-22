using FOS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FOS.API.Controllers
{
    public class APIsController : ApiController
    {
        ICrawlLinksService _craw;
        IExternalServiceFactory _factory;
        public APIsController(ICrawlLinksService craw, IExternalServiceFactory factory)
        {
            _craw = craw;
            _factory = factory;
        }
        // GET: api/APIs
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/APIs/5
        public Task<string> Get(int id)
        {
            return _factory.API(id);

            //return _craw.GetByIdAsync(id);
        }

        // POST: api/APIs
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/APIs/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/APIs/5
        public void Delete(int id)
        {
        }
    }
}
