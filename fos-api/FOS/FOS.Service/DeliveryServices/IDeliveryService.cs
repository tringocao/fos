using FOS.Model.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOS.Services.DeliveryServices
{
    public interface IDeliveryService
    {
        Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(int city_id, int restaurant_id);
        string GetExternalServiceById(int IdService);
        Task<DeliveryInfos> GetRestaurantFirstDeliveryInforAsync(int city_id, int restaurant_id);
        Task<List<DeliveryInfos>> GetRestaurantDeliveryInforByPagingAsync(int city_id, int pagenum, int pageSize);
        Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(int city_id, List<Restaurant> restaurant_ids);
        Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery);
        Task<DeliveryDetail> GetRestaurantDetailAsync(Restaurant restaurant);
    }
}