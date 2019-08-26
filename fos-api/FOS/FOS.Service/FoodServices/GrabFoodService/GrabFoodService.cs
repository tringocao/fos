using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Dto;

namespace FOS.Services.FoodServices.GrabFoodService
{
    public class GrabFoodService : IFoodService
    {
        public List<Food> GetFoods(DeliveryInfos delivery)
        {
            throw new NotImplementedException();
        }

        public List<Province> GetMetadata()
        {
            throw new NotImplementedException();
        }

        public string GetNameService()
        {
            throw new NotImplementedException();
        }

        public List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public List<Restaurant> GetRestaurants(Province province)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant)
        {
            throw new NotImplementedException();
        }
    }
}
