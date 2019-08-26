using FOS.Model.Dto;
using FOS.Services.ExternalServices;
using FOS.Services.ProvinceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.RestaurantServices
{
    public class RestaurantService : IRestaurantService
    {
        //IExternalServiceFactory _craw;
        IProvinceService _provinceService;
        int IdService;
        public RestaurantService(IProvinceService provinceService)
        {
            //_craw = craw;
            _provinceService = provinceService;
        }
        public string GetExternalServiceById(int IdService)
        {
            this.IdService = IdService;
            return "RestaurantService in " + _provinceService.GetExternalServiceById(IdService) + "is ready";
        }
        public List<Restaurant> GetRestaurantsByProvince(int city_id)
        {
            return _provinceService.GetRestaurants(_provinceService.GetMetadataById(city_id), "", null);
        }
        public List<Restaurant> GetRestaurantsByKeyword(int city_id, string keyword)
        {
            return _provinceService.GetRestaurants(_provinceService.GetMetadataById(city_id), keyword, null);
        }
        public List<Restaurant> GetRestaurantsByCategories(int city_id, List<RestaurantCategory> categories)
        {

            return _provinceService.GetRestaurants(_provinceService.GetMetadataById(city_id), "", categories);
        }
        public Restaurant GetRestaurantsById(int city_id, int restaurant_id)
        {
            var listRestaurants = GetRestaurantsByProvince(city_id);
            return listRestaurants.Where(p => p.restaurant_id == restaurant_id.ToString()).FirstOrDefault();
        }

        public List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant)
        {
            return _provinceService.GetRestaurantDeliveryInfor(restaurant);
        }

        public List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant)
        {
            return _provinceService.GetRestaurantsDeliveryInfor(restaurant);
        }
        public List<FoodCategory> GetFoodCatalogues(DeliveryInfos delivery)
        {
            return _provinceService.GetFoodCatalogues(delivery);
        }
    }
}
