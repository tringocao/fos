using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public interface IOrderDtoMapper
    {
        Model.Domain.Order ToModel(Dto.Order order);
        Dto.Order ToDto(Model.Domain.Order order);
    }

    public class OrderDtoMapper : IOrderDtoMapper
    {
        public Dto.Order ToDto(Model.Domain.Order order)
        {

            return new Dto.Order()
            {
                Id = order.Id.ToString(),
                IdDelivery = order.IdDelivery,
                IdRestaurant = order.IdRestaurant,
                IdUser = order.IdUser,
                OrderDate = order.OrderDate,
                FoodDetail = order.FoodDetail
            };
        }

        public Model.Domain.Order ToModel(Dto.Order order)
        {
            return new Domain.Order()
            {
                Id = Guid.Parse(order.Id),
                IdDelivery = order.IdDelivery,
                IdRestaurant = order.IdRestaurant,
                IdUser = order.IdUser,
                OrderDate = order.OrderDate,
                FoodDetail = order.FoodDetail
            };
        }
    }
}
