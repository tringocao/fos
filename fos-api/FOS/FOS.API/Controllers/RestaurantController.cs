using FOS.Model.Dto;
using FOS.Services.ExternalServices;
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
        public string GetIds(int IdService, int province_id)
        {
            _craw.GetExternalServiceById(IdService);
            return JsonConvert.SerializeObject(_craw.GetRestaurantsByProvince(province_id).Select(l => Int32.Parse(l.restaurant_id)));
        }

        // GET: api/Restaurant/5
        [HttpGet]
        [Route("GetById")]
        public string GetById(int IdService, int province_id, int restaurant_id)
        {
            _craw.GetExternalServiceById(IdService);

            return JsonConvert.SerializeObject(_craw.GetRestaurantsById(province_id, restaurant_id));
        }
        [HttpGet]
        [Route("GetByKeyword")]
        public string GetByKeyword(int IdService, int city_id, string keyword)
        {
            _craw.GetExternalServiceById(IdService);
            return JsonConvert.SerializeObject(_craw.GetRestaurantsByKeyword(city_id, keyword));
        }

        // POST: api/Restaurant
        public void Post([FromBody]string value)
        {
        }
        [HttpPut]
        [Route("PutCategorySearch")]
        public string PutCategorySearch(int IdService, int city_id, [FromBody]dynamic obj)
        {
            _craw.GetExternalServiceById(IdService);
            List<RestaurantCategory> categories = obj.ToObject<List<RestaurantCategory>>();
            return JsonConvert.SerializeObject(_craw.GetRestaurantsByCategories(city_id, categories).Select(l => Int32.Parse(l.restaurant_id)));
        }

        // DELETE: api/Restaurant/5
        public void Delete(int id)
        {
        }
    }
}
