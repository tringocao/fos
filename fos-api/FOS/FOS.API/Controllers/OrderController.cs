using FOS.API.App_Start;
using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services;
using FOS.Services.OrderServices;
using FOS.Services.SPListService;
using FOS.Services.SPUserService;
using Newtonsoft.Json;
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
        private readonly ISPListService _spListService;
        private readonly ISPUserService _spUserService;
        IGraphUserDtoMapper _graphUserDtoMapper;
        public OrderController(IOrderDtoMapper mapper,
            IOrderService service,
            ISPListService spListService,
            ISPUserService spUserService,
            IGraphUserDtoMapper graphUserDtoMapper)
        {
            _orderDtoMapper = mapper;
            _orderService = service;
            _spListService = spListService;
            _spUserService = spUserService;
            _graphUserDtoMapper = graphUserDtoMapper;
        }
        [HttpGet]
        [Route("GetById")]
        public  ApiResponse<Model.Dto.Order> GetById(string orderId)
        {
            try
            {
                Guid id = Guid.Parse(orderId);
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
        [Route("GetAllByEventId")]
        public ApiResponse<List<Model.Dto.Order>> GetAllByEventId(string eventId)
        {
            try
            {
                var orders = _orderService.GetOrders(eventId);
                return ApiUtil<List<Model.Dto.Order>>.CreateSuccessfulResult(
                    orders.Select(_order => 
                    _orderDtoMapper.ToDto(_order)).ToList()
               );
            }
            catch (Exception e)
            {
                return ApiUtil<List<Model.Dto.Order>>.CreateFailResult(e.ToString());
            }
        }

        [HttpGet]
        [Route("GetByEventvsUserId")]
        public ApiResponse<Model.Dto.Order> GetByEventvsUserId(string eventId, string userId)
        {
            try
            {
                return ApiUtil<Model.Dto.Order>.CreateSuccessfulResult(
                    _orderDtoMapper.ToDto(_orderService.GetByEventvsUserId(eventId, userId))
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
        [Route("AddWildOrder")]
        public async Task<ApiResponse> AddWildOrder([FromBody]Model.Dto.Order order)
        {
            try
            {
                Guid idOrder = Guid.NewGuid();
                order.Id = idOrder.ToString();

                var user = await _spUserService.GetCurrentUser();

                _orderService.CreateWildOrder(_orderDtoMapper.ToModel(order));
                GraphUser _user = new GraphUser()
                {
                    Id = order.IdUser,
                    DisplayName = user.DisplayName,
                    Mail = user.Mail,
                    UserPrincipalName = user.UserPrincipalName,
                };
                await _spListService.UpdateEventParticipant(order.IdEvent, _user);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
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
