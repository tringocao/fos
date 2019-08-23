using FOS.Model.Dto;
using System.Collections.Generic;

namespace FOS.Services.FoodServices
{
    public interface IExternalServiceFactory
    {
        string GetFoodServiceById(int id);
        List<Province> GetMetadata();
        

        List<Restaurant> GetRestaurants(Province province);

        List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant);
        List<Food> GetFoods(DeliveryInfos delivery);
        List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant);

        //IFoodService GetFoodService(APIs api);
    }
}