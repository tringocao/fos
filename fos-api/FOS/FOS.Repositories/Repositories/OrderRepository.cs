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
        bool UpdateOrder(DataModel.Order order);
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
                updateOrder = order;
                _context.SaveChanges();
                return true;
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
        public IEnumerable<DataModel.Order> GetAllOrder()
        {
            var list = _context.Orders.ToList();
            return list;

        }

    
    }
}
