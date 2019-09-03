using FOS.API.App_Start;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Services.ExternalServices;
using FOS.Services.RestaurantServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
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
        [Route("GetByKeywordLimit")]
        public string GetByKeywordLimit(int IdService, int city_id, string keyword, int limit)
        {
            _craw.GetExternalServiceById(IdService);
            return JsonConvert.SerializeObject(_craw.GetRestaurantsByKeyword(city_id, keyword).Select(l => Int32.Parse(l.restaurant_id)).Take(limit));
        }
        [HttpGet]
        [Route("GetMetadataForCategory")]
        public string GetMetadataForCategory(int IdService)
        {
            _craw.GetExternalServiceById(IdService);
            return System.Text.RegularExpressions.Regex.Unescape(JsonConvert.SerializeObject(_craw.GetMetadataForCategory()));
        }
        // POST: api/Restaurant
        public void Post([FromBody]string value)
        {
        }
        [HttpPut]
        [Route("PutCategorySearch")]
        public string PutCategorySearch(int IdService, int city_id, string keyword, [FromBody]dynamic categories)
        {
            _craw.GetExternalServiceById(IdService);
            if (categories.categories == null) return "";
            if (categories.categories.Count < 1)
            {
                return JsonConvert.SerializeObject(_craw.GetRestaurantsByKeyword(city_id, keyword).Select(l => Int32.Parse(l.restaurant_id)));
            }
            List<RestaurantCategory> newList = new List<RestaurantCategory>();
            JsonDtoMapper<RestaurantCategory> map = new JsonDtoMapper<RestaurantCategory>();
            foreach (var category in categories.categories)//get the fisrt catalogue
            {
                newList.Add(map.ToDto(category));
            }

            return JsonConvert.SerializeObject(_craw.GetRestaurantsByCategoriesKeyword(city_id, newList, keyword).Select(l => Int32.Parse(l.restaurant_id)));
        }

        // DELETE: api/Restaurant/5
        public void Delete(int id)
        {
        }
    }
}
