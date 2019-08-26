using FOS.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.FoodServices
{
    public interface IFoodService
    {
        string GetNameService();
        List<Restaurant> GetRestaurants(Province province);
        List<Food> GetFoods(DeliveryInfos delivery);
        List<Province> GetMetadata();
        List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant);
        List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant);
    }
}
