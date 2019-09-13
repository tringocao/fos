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
            List<FoodDetailJson> temp;
            if (order.FoodDetail == null)
            {
                temp = new List<FoodDetailJson>();
            }
            else
            {
                temp = order.FoodDetail.Select(x => new FoodDetailJson() { IdFood = x.Key.ToString(), Value = x.Value == null ? new Dictionary<string, string>() : x.Value }).ToList();
            }
            return new Dto.Order()
            {
                Id = order.Id.ToString(),
                IdDelivery = order.IdDelivery,
                IdRestaurant = order.IdRestaurant,
                IdUser = order.IdUser,
                OrderDate = order.OrderDate,
                FoodDetail = temp,
                IdEvent = order.IdEvent
            };
        }

        public Model.Domain.Order ToModel(Dto.Order order)
        {
            Dictionary<int, Dictionary<string, string>> foodDetail = new Dictionary<int, Dictionary<string, string>>();
            foreach (var food in order.FoodDetail)
            {
                foodDetail.Add(Int32.Parse(food.IdFood), food.Value);
            }
            return new Domain.Order()
            {
                Id = Guid.Parse(order.Id),
                IdDelivery = order.IdDelivery,
                IdRestaurant = order.IdRestaurant,
                IdUser = order.IdUser,
                OrderDate = order.OrderDate,
                FoodDetail = foodDetail,
                IdEvent = order.IdEvent

            };
        }
    }
}
