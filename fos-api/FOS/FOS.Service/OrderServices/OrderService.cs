using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model;
using FOS.Repositories.Repositories;

namespace FOS.Services.OrderServices
{
  
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public bool AddOrder(Model.Domain.Order order)
        {
            
        }

        public Repositories.DataModel.Order GetOrder(int id)
        {
            return _repository.GetOrder(id);
        }
    }
}
