using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Repositories.DataModel;
using FOS.Model;
using AutoMapper;

namespace FOS.Repositories.Repositories
{
    public interface IOrderRepository
    {
        bool AddOrder(DataModel.Order order);
        DataModel.Order GetOrder(Guid id);
        IEnumerable<Order> GetAllOrder();
        IEnumerable<DataModel.Order> GetAllOrderByEventId(string eventId);
        bool UpdateOrder(DataModel.Order order);
        IEnumerable<Model.Domain.UserNotOrder> GetUserNotOrdered(string eventId);
        DataModel.Order GetOrderByEventIdvsUserId(string eventid, string userId);
        bool DeleteOrderByIdEvent(string idEvent);
        IEnumerable<DataModel.Order> GetOrdersOfSpecificRestaurant(string restaurantId, string deliveryId);
        List<Model.Domain.UserNotOrderEmail> GetUserNotOrderEmail(string eventId);
        List<Model.Domain.UserNotOrderEmail> GetUserAlreadyOrderEmail(string eventId);
        bool DeleteOrderByUserId(string idUser, string idEvent);
        bool UpdateOrderStatusByOrderId(string OrderId, int OrderStatus);
        bool UpdateFoodDetailByOrderId(string OrderId, string FoodDetail);
        Model.Domain.Order GetOrderByEventIdAndMail(string EventId, string Mail);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly FosContext _context;
        public OrderRepository(FosContext context)
        {
            _context = context;
        }

        public bool AddOrder(DataModel.Order order)
        {
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateOrder(DataModel.Order order)
        {
            try
            {
                Order updateOrder = _context.Orders.FirstOrDefault(o => o.Id == order.Id);
                {
                    _context.Entry(updateOrder).CurrentValues.SetValues(order);
                    _context.SaveChanges();
                    return true;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataModel.Order GetOrder(Guid id)
        {
            return _context.Orders.Find(id.ToString());
        }
        public DataModel.Order GetOrderByEventIdvsUserId(string eventid, string userId)
        {
            return _context.Orders.Where(order => order.IdEvent == eventid && order.IdUser == userId).FirstOrDefault();
        }
        public IEnumerable<DataModel.Order> GetAllOrder()
        {
            var list = _context.Orders.ToList();
            return list;

        }

        public IEnumerable<DataModel.Order> GetAllOrderByEventId(string eventId)
        {
            var list = _context.Orders.Where(order => order.IdEvent == eventId).ToList();
            return list;
        }

        public IEnumerable<Model.Domain.UserNotOrder> GetUserNotOrdered(string eventId)
        {
            var orders = _context.Orders.Where(order =>
            order.IdEvent == eventId && order.FoodDetail.Length == 0 && order.OrderStatus == 0).ToList();
            var result = new List<Model.Domain.UserNotOrder>();
            foreach (var order in orders)
            {
                var item = new Model.Domain.UserNotOrder();
                item.OrderId = order.Id;
                item.UserId = order.IdUser;
                result.Add(item);
            }

            return result;
        }
        public bool DeleteOrderByIdEvent(string idEvent)
        {
            try
            {
                var orders = (from order in _context.Orders
                              where order.IdEvent == idEvent
                              select order).ToList();
                if (orders != null)
                {
                    _context.Orders.RemoveRange(orders);
                    _context.SaveChanges();
                }
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IEnumerable<DataModel.Order> GetOrdersOfSpecificRestaurant(string restaurantId, string deliveryId)
        {
            var deliveryIdInt = Int32.Parse(deliveryId);
            var restaurantIdInt = Int32.Parse(restaurantId);
            var result = _context.Orders.Where(o => o.IdDelivery == deliveryIdInt
                    && o.IdRestaurant == restaurantIdInt).ToList();
            return result;
        }
        public List<Model.Domain.UserNotOrderEmail> GetUserNotOrderEmail(string eventId)
        {
            var orders = _context.Orders.Where(order =>
            order.IdEvent == eventId && order.FoodDetail.Length == 0 && order.OrderStatus == 0).ToList();

            var result = new List<Model.Domain.UserNotOrderEmail>();
            foreach (var order in orders)
            {
                var item = new Model.Domain.UserNotOrderEmail();
                item.OrderId = order.Id;
                item.UserEmail = order.Email;
                result.Add(item);
            }

            return result;
        }

        public List<Model.Domain.UserNotOrderEmail> GetUserAlreadyOrderEmail(string eventId)
        {
            var orders = _context.Orders.Where(order =>
            order.IdEvent == eventId && order.FoodDetail.Length != 0).ToList();

            var result = new List<Model.Domain.UserNotOrderEmail>();
            foreach (var order in orders)
            {
                var item = new Model.Domain.UserNotOrderEmail();
                item.OrderId = order.Id;
                item.UserEmail = order.Email;
                result.Add(item);
            }

            return result;
        }

        public bool DeleteOrderByUserId(string idUser, string idEvent)
        {
            try
            {
                Order user = _context.Orders.First(o => o.IdUser == idUser
                && o.IdEvent == idEvent);
                _context.Orders.Remove(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateOrderStatusByOrderId(string OrderId, int OrderStatus)
        {
            try
            {
                using (var db = _context)
                {
                    var result = db.Orders.SingleOrDefault(o => o.Id == OrderId);
                    if (result != null)
                    {
                        result.OrderStatus = OrderStatus;
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateFoodDetailByOrderId(string OrderId, string FoodDetail)
        {
            try
            {
                using (var db = _context)
                {
                    var result = db.Orders.SingleOrDefault(o => o.Id == OrderId);
                    if (result != null)
                    {
                        result.FoodDetail = FoodDetail;
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public Model.Domain.Order GetOrderByEventIdAndMail(string EventId, string Mail)
        {
            try
            {
                Model.Domain.Order orderInfo = new Model.Domain.Order();
                var result = _context.Orders.SingleOrDefault(o => o.IdEvent == EventId && o.Email == Mail);
                if (result != null)
                {
                    orderInfo.OrderStatus = result.OrderStatus;
                    orderInfo.Id = new Guid(result.Id);
                    orderInfo.IdEvent = result.IdEvent;
                    _context.SaveChanges();
                }
                return orderInfo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
