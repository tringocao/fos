using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services;
using FOS.Services.OrderServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FOS.API.Controllers
{
    [LogActionWebApiFilter]
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        private readonly IOrderDtoMapper _orderDtoMapper;
        private readonly IOrderService _orderService;
        public OrderController(IOrderDtoMapper mapper, IOrderService service)
        {
            _orderDtoMapper = mapper;
            _orderService = service;
        }
        [HttpGet]
        [Route("GetById")]
        public  ApiResponse<Model.Dto.Order> GetById(string orderId)
        {
            try
            {
                Guid id = Guid.Parse(orderId);
                Order order = _orderService.GetOrder(id);
                return ApiUtil<Model.Dto.Order>.CreateSuccessfulResult(
                    _orderDtoMapper.ToDto(_orderService.GetOrder(id))
               );
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.Order>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("SendOrderById")]
        public ApiResponse<Model.Dto.Order> GetdById(string orderId)
        {
            try
            {
                Guid id = Guid.Parse(orderId);
                Order order = _orderService.GetOrder(id);
                return ApiUtil<Model.Dto.Order>.CreateSuccessfulResult(
                    _orderDtoMapper.ToDto(_orderService.GetOrder(id))
               );
            }
            catch (Exception e)
            {
                return ApiUtil<Model.Dto.Order>.CreateFailResult(e.ToString());
            }
        }

        [HttpGet]
        [Route("GetUserNotOrdered")]
        public ApiResponse<IEnumerable<Model.Dto.UserNotOrder>> GetUserNotOrdered(string eventId)
        {
            try
            {
                var result = _orderService.GetUserNotOrdered(eventId);
                return ApiUtil<IEnumerable<Model.Dto.UserNotOrder>>.CreateSuccessfulResult(result);
            }
            catch (Exception e)
            {
                return ApiUtil<IEnumerable<Model.Dto.UserNotOrder>>.CreateFailResult(e.ToString());
            }
        }

        [HttpPost]
        [Route("UpdateOrder")]
        public ApiResponse UpdateOrder([FromBody]Model.Dto.Order order)
        {
            try
            {
               
                _orderService.UpdateOrder(_orderDtoMapper.ToModel(order));
                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }

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
