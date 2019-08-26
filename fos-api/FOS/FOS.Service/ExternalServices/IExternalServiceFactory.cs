using FOS.Model.Dto;
using System.Collections.Generic;

namespace FOS.Services.ExternalServices
{
    public interface IExternalServiceFactory
    {
        string GetExternalServiceById(int id);
        List<Province> GetMetadata();
        List<Restaurant> GetRestaurants(Province province, string keyword, List<RestaurantCategory> category);

        List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant);
        List<FoodCategory> GetFoodCatalogues(DeliveryInfos delivery);
        List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant);
    }
}