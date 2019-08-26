using FOS.Services.FoodServices;
using FOS.Services.RestaurantServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [RoutePrefix("api/Restaurant")]

    public class RestaurantController : ApiController
    {
        IRestaurantService _craw;
        public RestaurantController(IRestaurantService craw)
        {
            _craw = craw;
        }
        // GET: api/Restaurant
        [HttpGet]
        [Route("GetIds")]
        public IEnumerable<int> GetIds(int IdService, int province_id)
        {
            _craw.GetFoodServiceById(IdService);
            return _craw.GetRestaurantsByProvince(province_id).Select(l => Int32.Parse(l.restaurant_id));
        }

        // GET: api/Restaurant/5
        [HttpGet]
        [Route("GetById")]
        public string GetById(int IdService, int province_id, int restaurant_id)
        {
            _craw.GetFoodServiceById(IdService);

            return JsonConvert.SerializeObject(_craw.GetRestaurantsById(province_id, restaurant_id));
        }

        // POST: api/Restaurant
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Restaurant/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Restaurant/5
        public void Delete(int id)
        {
        }
    }
}
