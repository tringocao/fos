using FOS.Model.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOS.Services.ProvinceServices
{
    public interface IProvinceService
    {
        Task<Province> GetMetadataByIdAsync(int city_id);
        string GetExternalServiceById(int IdService);
        Task<List<Restaurant>> GetRestaurantsAsync(Province province, string keyword, List<RestaurantCategory> categories);
        Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(Restaurant restaurant);
        Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Restaurant> restaurant);
        Task<List<RestaurantCategory>> GetMetadataForCategoryAsync();

        Task<List<Province>> GetMetadataForProvinceAsync();
        Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery);

        Task<DeliveryDetail> GetRestaurantDetailAsync(Restaurant restaurant);

    }
}