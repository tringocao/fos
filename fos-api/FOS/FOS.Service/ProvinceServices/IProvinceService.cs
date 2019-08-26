using FOS.Model.Dto;
using System.Collections.Generic;

namespace FOS.Services.ProvinceServices
{
    public interface IProvinceService
    {
        Province GetMetadataById(int city_id);
        string GetFoodServiceById(int IdService);
        List<Restaurant> GetRestaurants(Province province);
        List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant);
        List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant);

        List<Province> GetMetadata();


    }
}