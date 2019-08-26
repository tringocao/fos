using FOS.Model.Dto;
using FOS.Services.DeliveryServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [RoutePrefix("api/Delivery")]
    public class DeliveryController : ApiController
    {
        IDeliveryService _craw;
        public DeliveryController(IDeliveryService craw)
        {
            _craw = craw;
        }
        // GET: api/Delivery
        [HttpGet]
        [Route("Get")]
        public string Get(int IdService, int city_id, int restaurant_id)
        {
            _craw.GetFoodServiceById(IdService);
            return JsonConvert.SerializeObject(_craw.GetRestaurantDeliveryInfor(city_id, restaurant_id));
        }

        // GET: api/Delivery/5
        [HttpGet]
        [Route("GetFirstId")]
        public string GetFirstId(int IdService, int city_id, int restaurant_id)
        {
            _craw.GetFoodServiceById(IdService);

            return JsonConvert.SerializeObject(_craw.GetRestaurantFirstDeliveryInfor(city_id, restaurant_id));
        }
        [HttpGet]
        [Route("GetPageDelivery")]
        public string GetPageDelivery(int IdService, int city_id, int pagenum, int pagesize)
        {
            _craw.GetFoodServiceById(IdService);

            return JsonConvert.SerializeObject(_craw.GetRestaurantDeliveryInforByPaging(city_id, pagenum, pagesize));
        }
        // POST: api/Delivery
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Delivery/5
        [HttpPut]
        [Route("PutRestaurantIds")]
        // PUT: api/Delivery/5
        public string PutRestaurantIds(int IdService, int city_id, [FromBody]dynamic data)
        {
            _craw.GetFoodServiceById(IdService);
            List<Restaurant> newList = new List<Restaurant>();
            foreach (var id in data.restaurant_ids)//get the fisrt catalogue
            {
                Restaurant item = new Restaurant();
                item.restaurant_id = id.ToString();
                newList.Add(item);
            }
            return JsonConvert.SerializeObject(_craw.GetRestaurantsDeliveryInfor(city_id, newList));

        }

        // DELETE: api/Delivery/5
        public void Delete(int id)
        {
        }
    }
}
