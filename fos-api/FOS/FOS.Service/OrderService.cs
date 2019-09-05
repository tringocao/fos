using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model;
using FOS.Repositories;

namespace FOS.Services
{
    public interface IOrderService
    {
        bool AddOrder(Model.Order order);
        Repositories.DataModel.Order GetOrder(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public bool AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public Repositories.DataModel.Order GetOrder(int id)
        {
            return _repository.GetOrder(id);
        }
    }
}
