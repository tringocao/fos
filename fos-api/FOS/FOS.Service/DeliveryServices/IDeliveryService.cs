using FOS.Model.Dto;
using System.Collections.Generic;

namespace FOS.Services.DeliveryServices
{
    public interface IDeliveryService
    {
        List<DeliveryInfos> GetRestaurantDeliveryInfor(int city_id, int restaurant_id);
        string GetExternalServiceById(int IdService);
        DeliveryInfos GetRestaurantFirstDeliveryInfor(int city_id, int restaurant_id);
        List<DeliveryInfos> GetRestaurantDeliveryInforByPaging(int city_id, int pagenum, int pageSize);
        List<DeliveryInfos> GetRestaurantsDeliveryInfor(int city_id, List<Restaurant> restaurant_ids);
        List<FoodCategory> GetFoodCatalogues(DeliveryInfos delivery);

    }
}