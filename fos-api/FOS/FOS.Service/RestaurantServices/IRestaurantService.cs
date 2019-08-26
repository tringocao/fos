using FOS.Model.Dto;
using System.Collections.Generic;

namespace FOS.Services.RestaurantServices
{
    public interface IRestaurantService
    {
        string GetExternalServiceById(int IdService);
        List<Restaurant> GetRestaurantsByProvince(int city_id);
        Restaurant GetRestaurantsById(int city_id, int restaurant_id);
        List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant);
        List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant);
        List<FoodCatalogue> GetFoodCatalogues(DeliveryInfos delivery);

    }
}