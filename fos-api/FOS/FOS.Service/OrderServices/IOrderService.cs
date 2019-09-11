using FOS.Model.Domain;
using System;

namespace FOS.Services.OrderServices
{
    public interface IOrderService
    {
        bool CreateOrderWithEmptyFoods(Guid id, string UserId, string RestaurantId, string DeliveyId, string EventId);
        Order GetOrder(Guid id);
        bool UpdateOrder(Order order);
    }
}