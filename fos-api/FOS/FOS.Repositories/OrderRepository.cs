using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model;

namespace FOS.Repositories
{
    public interface IOrderRepository
    {
        bool AddOrder(Model.Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly FosContext _context;
        public OrderRepository(FosContext context)
        {
            _context = context;
        }

        public bool AddOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
