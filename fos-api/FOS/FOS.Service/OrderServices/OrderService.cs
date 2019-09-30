using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model;
using FOS.Model.Domain;
using FOS.Repositories.Mapping;
using FOS.Repositories.Repositories;

namespace FOS.Services.OrderServices
{

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        IOrderMapper _orderMapper;
        public OrderService(IOrderRepository repository, IOrderMapper orderMapper)
        {
            _repository = repository;
            _orderMapper = orderMapper;
        }
        public bool UpdateOrder(Order order)
        {
            Repositories.DataModel.Order efOrder = new Repositories.DataModel.Order();
            _orderMapper.MapToEfObject(efOrder, order);
            _repository.UpdateOrder(efOrder);
            return true;
        }

        public bool CreateOrderWithEmptyFoods(Guid id, string UserId, string RestaurantId, string DeliveyId, string EventId, string Email, int OrderStatus)
        {
            Repositories.DataModel.Order efOrder = new Repositories.DataModel.Order();
            Order newOrder = new Order()
            {
                Id = id,
                IdDelivery = Int32.Parse(DeliveyId),
                IdRestaurant = Int32.Parse(RestaurantId),
                IdUser = UserId,
                IdEvent = EventId,
                OrderDate = DateTime.Now,
                Email = Email,
                OrderStatus = OrderStatus
            };
            _orderMapper.MapToEfObject(efOrder, newOrder);
            return _repository.AddOrder(efOrder);
        }
        public Order GetOrder(Guid id)
        {
            return _orderMapper.MapToDomain(_repository.GetOrder(id));
        }
        public IEnumerable<UserNotOrder> GetUserNotOrdered(string eventId)
        {
            return _repository.GetUserNotOrdered(eventId);
        }

        public List<Order> GetOrders(string eventId)
        {
            var orders = _repository.GetAllOrderByEventId(eventId);
            return orders.Select(order => _orderMapper.MapToDomain(order)).ToList();
        }

        public bool CreateWildOrder(Order order)
        {
            Repositories.DataModel.Order efOrder = new Repositories.DataModel.Order();
            _orderMapper.MapToEfObject(efOrder, order);
            return _repository.AddOrder(efOrder);
        }


        public Order GetByEventvsUserId(string eventId, string userId)
        {
            return _orderMapper.MapToDomain(_repository.GetOrderByEventIdvsUserId(eventId, userId));
        }
        public bool DeleteOrderByIdEvent(string idEvent)
        {
            return _repository.DeleteOrderByIdEvent(idEvent);
        }

        public List<UserNotOrderEmail> GetUserNotOrderEmail(string eventId)
        {
            return _repository.GetUserNotOrderEmail(eventId);
        }

        public List<UserNotOrderEmail> GetUserAlreadyOrderEmail(string eventId)
        {
            return _repository.GetUserAlreadyOrderEmail(eventId);
        }

        public bool DeleteOrderByUserId(string idUser, string idEvent)
        {
            return _repository.DeleteOrderByUserId(idUser,idEvent);
        }
        public async Task<bool> UpdateOrderStatusByOrderId(string OrderId, int OrderStatus)
        {
            return  _repository.UpdateOrderStatusByOrderId(OrderId, OrderStatus);
        }
    }
}
