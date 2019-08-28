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
        public List<FoodCategory> GetFoodFromDelivery(List<DeliveryInfos> deliveries)
        {
            throw new NotImplementedException();
        }

        public List<FoodCategory> GetFoodCatalogues(DeliveryInfos delivery)
        {
            throw new NotImplementedException();
        }

        public List<Province> GetMetadata()
        {
            throw new NotImplementedException();
        }

        public string GetNameService()
        {
            throw new NotImplementedException();
        }

        public List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public List<Restaurant> GetRestaurants(Province province)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant)
        {
            throw new NotImplementedException();
        }

        public List<Restaurant> GetRestaurants(Province province, string keyword, List<RestaurantCategory> category)
        {
            throw new NotImplementedException();
        }

        public List<Province> GetMetadataForProvince()
        {
            throw new NotImplementedException();
        }

        public List<RestaurantCategory> GetMetadataForCategory()
        {
            throw new NotImplementedException();
        }
    }
}
