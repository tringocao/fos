using FOS.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.ExternalServices
{
    public interface IExternalService
    {
        string GetNameService();
        Task<List<Restaurant>> GetRestaurantsAsync(Province province, string keyword, List<RestaurantCategory> category);
        Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery);
        Task<List<Province>> GetMetadataForProvinceAsync();
        Task<List<RestaurantCategory>> GetMetadataForCategoryAsync();
        Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(Restaurant restaurant);
        Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Restaurant> restaurant);
        Task<List<FoodCategory>> GetFoodFromDelivery(List<DeliveryInfos> deliveries);

    }
}
