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
        DataModel.Order GetOrder(int id);
        IEnumerable<Model.Domain.Order> GetAllOrder();
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
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DataModel.Order GetOrder(int id)
        {
            return _context.Orders.Find(id);
        }
        public IEnumerable<Model.Domain.Order> GetAllOrder()
        {
            var list = _context.Orders.ToList();
            return Mapper.Map<IEnumerable<DataModel.Order>, IEnumerable<Model.Domain.Order>>(list);

        }
    }
}
