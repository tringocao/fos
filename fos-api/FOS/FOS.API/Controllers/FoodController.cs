using FOS.Services.FoodServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    public class FoodController : ApiController
    {
        IFoodService _craw;
        public FoodController(IFoodService craw)
        {
            _craw = craw;
        }
        // GET: api/Food
        [HttpGet]
        [Route("GetFoodCatalogues")]
        public string GetFoodCatalogues(int IdService, int delivery_id)
        {
            _craw.GetExternalServiceById(IdService);
            return JsonConvert.SerializeObject(_craw.GetFoodCataloguesFromDeliveryId(delivery_id));
        }

        // GET: api/Food/5
        [HttpGet]
        [Route("GetFood")]
        public string GetFood(int IdService, int delivery_id, int dish_type_id)
        {
            _craw.GetExternalServiceById(IdService);
            return JsonConvert.SerializeObject(_craw.GetFoodFromCatalogue(delivery_id, dish_type_id));
        }

        // POST: api/Food
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Food/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Food/5
        public void Delete(int id)
        {
        }
    }
}
