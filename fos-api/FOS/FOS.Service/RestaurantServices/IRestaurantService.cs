using FOS.Model.Domain.NowModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOS.Services.RestaurantServices
{
    public interface IRestaurantService
    {
        string GetExternalServiceById(int idService);
        Task<List<Restaurant>> GetRestaurantsByProvinceAsync(int cityId);
        Task<Restaurant> GetRestaurantsByIdAsync(int cityId, int restaurant_id);
        Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(Restaurant restaurant);
        Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Restaurant> restaurant);
        Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery);
        Task<List<Restaurant>> GetRestaurantsByCategoriesAsync(int cityId, List<RestaurantCategory> categories);
        Task<List<Restaurant>> GetRestaurantsByKeywordAsync(int cityId, string keyword);
        Task<List<RestaurantCategory>> GetMetadataForCategoryAsync();
        Task<List<Restaurant>> GetRestaurantsByCategoriesKeywordAsync(int cityId, List<RestaurantCategory> categories, string keyword);
        Task<DeliveryDetail> GetDeliveryDetailAsync(Restaurant restaurant);
    }
}