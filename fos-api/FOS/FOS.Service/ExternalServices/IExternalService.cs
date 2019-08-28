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
        List<Restaurant> GetRestaurants(Province province, string keyword, List<RestaurantCategory> category);
        List<FoodCategory> GetFoodCatalogues(DeliveryInfos delivery);
        List<Province> GetMetadataForProvince();
        List<RestaurantCategory> GetMetadataForCategory();
        List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant);
        List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant);
        List<FoodCategory> GetFoodFromDelivery(List<DeliveryInfos> deliveries);

    }
}
