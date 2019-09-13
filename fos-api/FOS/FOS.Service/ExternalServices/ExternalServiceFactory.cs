using FOS.Model.Domain;
using FOS.Model.Domain.NowModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.ExternalServices
{
    public class ExternalServiceFactory : IExternalServiceFactory
    {
        private IFOSFoodServiceAPIsService _foodServiceAPIsService;
        IExternalService service;
        public ExternalServiceFactory(IFOSFoodServiceAPIsService foodServiceAPIsService)
        {
            this._foodServiceAPIsService = foodServiceAPIsService;
        }
        public string GetExternalServiceById(int id)
        {
            Apis apis = _foodServiceAPIsService.GetById(id);
            service = GetExternalService(apis);
            return service.GetNameService();
            //----------------Test----------------------
            //var re1 = service.GetMetadata().ToList().ToString();
            //Province province = new Province() { id = 217 };
            //var re2 = service.GetRestaurants(province).ToList().ToString();
            //Restaurant restaurant = new Restaurant() { restaurant_id = 595, deliveryId = 607 };
            //var re3 = service.GetRestaurantDeliveryInfor(restaurant);
            //var re4 = service.GetFoods(restaurant);
            //----------------Test----------------------
        }
        public async Task<List<Province>> GetMetadataForProvinceAsync()
        {
            return await service.GetMetadataForProvinceAsync();
        }
        public async Task<List<RestaurantCategory>> GetMetadataForCategoryAsync()
        {
            return await service.GetMetadataForCategoryAsync();
        }
        public async Task<List<Restaurant>> GetRestaurantsAsync(Province province, string keyword, List<RestaurantCategory> category)
        {
            return await service.GetRestaurantsAsync(province, keyword, category);
        }
        public async Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(Restaurant restaurant)
        {
            return await service.GetRestaurantDeliveryInforAsync(restaurant);
        }
        public async Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Restaurant> restaurant)
        {
            return await service.GetRestaurantsDeliveryInforAsync(restaurant);
        }
        public async Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery)
        {
            return await service.GetFoodCataloguesAsync(delivery);
        }
        public async Task<DeliveryDetail> GetRestaurantDetailAsync(Restaurant restaurant)
        {
            return await service.GetRestaurantDetailAsync(restaurant);
        }
        private IExternalService GetExternalService(Apis api)
        {
            switch (api.TypeService)
            {
                case ServiceKind.Now:
                    {
                        return new NowService.NowService(api);
                    }
                case ServiceKind.GrabFood:
                    {
                        return new GrabFoodService.GrabFoodService();
                    }
                default:
                    throw new Exception();
            }
        }

       
    }
}
