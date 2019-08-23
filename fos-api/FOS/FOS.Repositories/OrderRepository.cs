using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Repositories.DataModel;
using FOS.Model;

namespace FOS.Repositories
{
    public interface IOrderRepository
    {
        bool AddOrder(Model.Order order);
        Repositories.DataModel.Order GetOrder(int id);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly FosContext _context;
        public OrderRepository(FosContext context)
        {
            _context = context;
        }

        public bool AddOrder(Model.Order order)
        {
            throw new NotImplementedException();
        }

        public Repositories.DataModel.Order GetOrder(int id)
        {
            return _context.Orders.Find(id);
        }
    }
}
