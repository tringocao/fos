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
        List<Restaurant> GetRestaurants(Province province);
        List<FoodCatalogue> GetFoodCatalogues(DeliveryInfos delivery);
        List<Province> GetMetadata();
        List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant);
        List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant);
        List<FoodCatalogue> GetFoodFromDelivery(List<DeliveryInfos> deliveries);

    }
}
