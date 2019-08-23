using FOS.Services.FoodServices;
using FOS.Services.ProvinceServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [RoutePrefix("api/Province")]

    public class ProvinceController : ApiController
    {
        IProvinceService _craw;
        public ProvinceController(IProvinceService craw)
        {
            _craw = craw;
        }
        // GET: api/Province 
        [HttpGet]
        [Route("Get")]
        public string Get(int IdService)
        {
            _craw.GetFoodServiceById(IdService);
            return JsonConvert.SerializeObject(_craw.GetMetadata());
        }

        // GET: api/Province/5
        [HttpGet]
        [Route("GetById")]
        public string GetById(int IdService, int id)
        {
            _craw.GetFoodServiceById(IdService);
           
            return JsonConvert.SerializeObject(_craw.GetMetadataById(id));
        }

        // POST: api/Province
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Province/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Province/5
        public void Delete(int id)
        {
        }
    }
}
