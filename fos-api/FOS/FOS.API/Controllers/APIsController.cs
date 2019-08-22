using FOS.Model.Dto;
using FOS.Services;
using FOS.Services.FoodServices;
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
        IExternalServiceFactory _craw;
        public APIsController(IExternalServiceFactory craw)
        {
            _craw = craw;
        }
        // GET: api/APIs
        public IEnumerable<string> GetALLRestaurants(int idService)
        {
            _craw.Service(idService);

            return new string[] { "value1", "value2" };

            //ServiceKind foodService = (ServiceKind)idService;
            //IFoodService myService = ExternalServiceFactory.GetFoodService(foodService);
            //return myService.GetRestaurantsAsync();

        }
        // GET: api/APIs/5
        [HttpGet]
        public string Gets(int id)
        {
            return "";
        }
        // GET: api/APIs/5
        public Task<string> GetRestaurant(int id)
        {
            //return _factory.API(id);

            return null;
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
