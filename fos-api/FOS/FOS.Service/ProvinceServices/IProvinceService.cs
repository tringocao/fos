using FOS.Model.Dto;
using System.Collections.Generic;

namespace FOS.Services.ProvinceServices
{
    public interface IProvinceService
    {
        Province GetMetadataById(int city_id);
        string GetExternalServiceById(int IdService);
        List<Restaurant> GetRestaurants(Province province, string keyword, List<RestaurantCategory> categories);
        List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant);
        List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant);
        List<RestaurantCategory> GetMetadataForCategory();

        List<Province> GetMetadataForProvince();
        List<FoodCategory> GetFoodCatalogues(DeliveryInfos delivery);


    }
}