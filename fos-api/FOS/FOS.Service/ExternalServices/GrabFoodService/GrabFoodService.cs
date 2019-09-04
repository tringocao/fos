using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Dto;

namespace FOS.Services.ExternalServices.GrabFoodService
{
    public class GrabFoodService : IExternalService
    {
        public Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery)
        {
            throw new NotImplementedException();
        }

        public Task<List<FoodCategory>> GetFoodFromDelivery(List<DeliveryInfos> deliveries)
        {
            throw new NotImplementedException();
        }

        public Task<List<RestaurantCategory>> GetMetadataForCategoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Province>> GetMetadataForProvinceAsync()
        {
            throw new NotImplementedException();
        }

        public string GetNameService()
        {
            throw new NotImplementedException();
        }

        public Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public Task<List<Restaurant>> GetRestaurantsAsync(Province province, string keyword, List<RestaurantCategory> category)
        {
            throw new NotImplementedException();
        }

        public Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Restaurant> restaurant)
        {
            throw new NotImplementedException();
        }
    }
}
