using FOS.Model.Domain.NowModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOS.Services.ExternalServices
{
    public interface IExternalServiceFactory
    {
        string GetExternalServiceById(int id);
        Task<List<Province>> GetMetadataForProvinceAsync();
        Task<List<RestaurantCategory>> GetMetadataForCategoryAsync();
        Task<List<Restaurant>> GetRestaurantsAsync(Province province, string keyword, List<RestaurantCategory> category);

        Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(Restaurant restaurant);
        Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery);
        Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Restaurant> restaurant);
        Task<DeliveryDetail> GetRestaurantDetailAsync(Restaurant restaurant);
    }
}