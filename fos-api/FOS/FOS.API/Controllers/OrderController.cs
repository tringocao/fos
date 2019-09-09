using FOS.API.App_Start;
using FOS.Model.Mapping;
using FOS.Services;
using FOS.Services.OrderServices;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    public class OrderController : ApiController
    {
        private readonly IOrderDtoMapper _mapper;
        private readonly IOrderService _service;
        public OrderController(IOrderDtoMapper mapper, IOrderService service)
        {
            _mapper = mapper;
            _service = service;
        }
        // GET: api/Order
        //public IEnumerable<Model.Dto.Order> Get()
        //{
        //    return new List<Model.Dto.Order>();
        //}

        //// GET: api/Order/5
        //public Model.Dto.Order Get(int id)
        //{
        //    return new Model.Dto.Order();
        //}

        //// POST: api/Order
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Order/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Order/5
        //public void Delete(int id)
        //{
        //}

        //public Repositories.DataModel.Order GetOrder(int id)
        //{
        //    //return null;
        //    return _service.GetOrder(id);
        //}
    }
}
