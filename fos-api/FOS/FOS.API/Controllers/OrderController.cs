using FOS.API.App_Start;
using FOS.Common.Constants;
using FOS.Model.Domain;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Model.Util;
using FOS.Services;
using FOS.Services.EventServices;
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
        private readonly IUserNotOrderEmailDtoMapper _userNotOrderEmailDtoMapper;
        private readonly IUserNotOrderDtoMapper _userNotOrderDtoMapper;
        private readonly IEventService _eventService;

        public OrderController(IOrderDtoMapper mapper,
            IOrderService service,
            ISPListService spListService,
            ISPUserService spUserService,
            IGraphUserDtoMapper graphUserDtoMapper,
            IUserNotOrderEmailDtoMapper userNotOrderEmailDtoMapper,
            IUserNotOrderDtoMapper userNotOrderDtoMapper,
            IEventService eventService)
        {
            _orderDtoMapper = mapper;
            _orderService = service;
            _spListService = spListService;
            _spUserService = spUserService;
            _graphUserDtoMapper = graphUserDtoMapper;
            _userNotOrderEmailDtoMapper = userNotOrderEmailDtoMapper;
            _userNotOrderDtoMapper = userNotOrderDtoMapper;
            _eventService = eventService;
        }
        [HttpGet]
        [Route("GetById")]
        public ApiResponse<Model.Dto.Order> GetById(string orderId)
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
        [Route("GetOrderIdOfUserInEvent")]
        public ApiResponse<string> GetOrderIdOfUserInEvent(string eventId, string userId)
        {
            try
            {
                var result = _orderService.GetOrderIdOfUserInEvent(eventId, userId);
                return ApiUtil<string>.CreateSuccessfulResult(result);
            }
            catch (Exception e)
            {
                return ApiUtil<string>.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetUserNotOrdered")]
        public async Task<ApiResponse<IEnumerable<Model.Dto.UserNotOrder>>> GetUserNotOrdered(string eventId)
        {
            try
            {
                var id = Int32.Parse(eventId);
                var isHost = await _spUserService.ValidateIsHost(id);
                if (!isHost)
                {
                    return ApiUtil<IEnumerable<Model.Dto.UserNotOrder>>.CreateFailResult(Constant.UserNotPerission);
                }
                var user = _orderService.GetUserNotOrdered(eventId);
                var result = _userNotOrderDtoMapper.ListToDomain(user);
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
                order.OrderStatus = EventEmail.Ordered;
                var user = await _spUserService.GetCurrentUser();

                await _orderService.CreateWildOrder(_orderDtoMapper.ToModel(order));
                Model.Dto.GraphUser _user = new Model.Dto.GraphUser()
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
        public async Task<ApiResponse> UpdateOrder([FromBody]Model.Dto.Order order)
        {
            try
            {
                var eventId = Int32.Parse(order.IdEvent);
                var result = _eventService.GetEvent(eventId);
                if(result.Status == EventStatus.Closed)
                {
                    var isHost = await _spUserService.ValidateIsHost(eventId);
                    if (!isHost)
                    {
                        return ApiUtil.CreateFailResult(Constant.UserNotPerission);
                    }
                }

                _orderService.UpdateOrder(_orderDtoMapper.ToModel(order));
                return ApiUtil.CreateSuccessfulResult();

            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("GetUserNotOrderedEmail")]
        public ApiResponse<List<Model.Dto.UserNotOrderEmail>> GetUserNotOrderedEmail(string eventId)
        {
            try
            {
                var userNotOrderEmail = _orderService.GetUserNotOrderEmail(eventId);
                var userNotOrderEmailDTO = userNotOrderEmail.Select(
                    user => _userNotOrderEmailDtoMapper.ToDto(user)
                ).ToList();
                return ApiUtil<List<Model.Dto.UserNotOrderEmail>>.CreateSuccessfulResult(userNotOrderEmailDTO);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        [HttpGet]
        [Route("UpdateOrderStatusByOrderId")]
        public async Task<ApiResponse> UpdateOrderStatusByOrderId(string OrderId, int OrderStatus)
        {
            try
            {
                await _orderService.UpdateOrderStatusByOrderId(OrderId, OrderStatus);
                return ApiUtil.CreateSuccessfulResult();
            }
            catch (Exception e)
            {
                return ApiUtil.CreateFailResult(e.ToString());
            }
        }
        [HttpGet]
        [Route("UpdateFoodDetailByOrderId")]
        public async Task<ApiResponse> UpdateFoodDetailByOrderId(string OrderId, string FoodDetail)
        {
            try
            {
                await _orderService.UpdateFoodDetailByOrderId(OrderId, FoodDetail);
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
