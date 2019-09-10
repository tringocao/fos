using FOS.Model.Domain.NowModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOS.Services.RestaurantServices
{
    public interface IRestaurantService
    {
        string GetExternalServiceById(int IdService);
        Task<List<Restaurant>> GetRestaurantsByProvinceAsync(int city_id);
        Task<Restaurant> GetRestaurantsByIdAsync(int city_id, int restaurant_id);
        Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(Restaurant restaurant);
        Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Restaurant> restaurant);
        Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery);
        Task<List<Restaurant>> GetRestaurantsByCategoriesAsync(int city_id, List<RestaurantCategory> categories);
        Task<List<Restaurant>> GetRestaurantsByKeywordAsync(int city_id, string keyword);
        Task<List<RestaurantCategory>> GetMetadataForCategoryAsync();
        Task<List<Restaurant>> GetRestaurantsByCategoriesKeywordAsync(int city_id, List<RestaurantCategory> categories, string keyword);
        Task<DeliveryDetail> GetDeliveryDetailAsync(Restaurant restaurant);
    }
}