using FOS.Model.Domain;
using System;
using System.Collections.Generic;

namespace FOS.Services.OrderServices
{
    public interface IOrderService
    {
        bool CreateWildOrder(Order order);
        bool CreateOrderWithEmptyFoods(Guid id, string UserId, string RestaurantId, string DeliveyId, string EventId);
        Order GetOrder(Guid id);
        bool UpdateOrder(Order order);
        IEnumerable<Model.Dto.UserNotOrder> GetUserNotOrdered(string eventId);
        Order GetByEventvsUserId(string eventId, string userId);
    }
}