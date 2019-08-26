using FOS.Model.Dto;
using System.Collections.Generic;

namespace FOS.Services.ProvinceServices
{
    public interface IProvinceService
    {
        Province GetMetadataById(int city_id);
        string GetExternalServiceById(int IdService);
        List<Restaurant> GetRestaurants(Province province, string keyword, List<RestaurantCategory> category);
        List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant);
        List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant);

        List<Province> GetMetadata();
        List<FoodCategory> GetFoodCatalogues(DeliveryInfos delivery);

    }
}