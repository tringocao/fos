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
        public bool CreateOrderWithEmptyFoods(Guid id, string UserId, string RestaurantId, string DeliveyId, string EventId)
        {
            Repositories.DataModel.Order efOrder = new Repositories.DataModel.Order();
            Order newOrder = new Order()
            {
                Id = id,
                IdDelivery = Int32.Parse(DeliveyId),
                IdRestaurant = Int32.Parse(RestaurantId),
                IdUser = UserId,
                IdEvent = EventId,
                OrderDate = DateTime.Now
            };
            _orderMapper.MapToEfObject(efOrder, newOrder);
            return _repository.AddOrder(efOrder);
        }
        public Order GetOrder(Guid id)
        {
            return _orderMapper.MapToDomain(_repository.GetOrder(id));
        }
    }
}
