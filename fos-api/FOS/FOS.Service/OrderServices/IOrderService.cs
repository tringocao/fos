using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOS.Services.OrderServices
{
    public interface IOrderService
    {
        bool CreateWildOrder(Order order);
        Task<bool> CreateOrderWithEmptyFoods(Guid id, string UserId, string RestaurantId, string DeliveyId, string EventId, string Email, int OrderStatus);
        Order GetOrder(Guid id);
        List<Order> GetOrders(string eventId);
        bool UpdateOrder(Order order);
        IEnumerable<UserNotOrder> GetUserNotOrdered(string eventId);
        Order GetByEventvsUserId(string eventId, string userId);
        bool DeleteOrderByIdEvent(string idEvent);
        List<UserNotOrderEmail> GetUserNotOrderEmail(string eventId);
        Task<List<UserNotOrderEmail>> GetUserAlreadyOrderEmail(string eventId);
        bool DeleteOrderByUserId(string idUser, string idEvent);
        Task<bool> UpdateOrderStatusByOrderId(string OrderId, int OrderStatus);
        Task<bool> UpdateFoodDetailByOrderId(string OrderId, string FoodDetail);
        Task<Order> GetOrderByEventIdAndMail(string EventId, string Mail);
    }
}