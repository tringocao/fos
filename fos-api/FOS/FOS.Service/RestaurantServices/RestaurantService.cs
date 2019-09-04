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
        public async Task<List<Restaurant>> GetRestaurantsByProvinceAsync(int city_id)
        {
            return await _provinceService.GetRestaurantsAsync(await _provinceService.GetMetadataByIdAsync(city_id), "", null);
        }
        public async Task<List<Restaurant>> GetRestaurantsByKeywordAsync(int city_id, string keyword)
        {
            return await _provinceService.GetRestaurantsAsync(await _provinceService.GetMetadataByIdAsync(city_id), keyword, null);
        }
        public async Task<List<Restaurant>> GetRestaurantsByCategoriesAsync(int city_id, List<RestaurantCategory> categories)
        {

            return await _provinceService.GetRestaurantsAsync(await _provinceService.GetMetadataByIdAsync(city_id), "", categories);
        }
        public async Task<List<Restaurant>> GetRestaurantsByCategoriesKeywordAsync(int city_id, List<RestaurantCategory> categories, string keyword)
        {

            return await _provinceService.GetRestaurantsAsync(await _provinceService.GetMetadataByIdAsync(city_id), keyword, categories);
        }
        public async Task<Restaurant> GetRestaurantsByIdAsync(int city_id, int restaurant_id)
        {
            var listRestaurants = await GetRestaurantsByProvinceAsync(city_id);
            return listRestaurants.Where(p => p.restaurant_id == restaurant_id.ToString()).FirstOrDefault();
        }

        public async Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(Restaurant restaurant)
        {
            return await _provinceService.GetRestaurantDeliveryInforAsync(restaurant);
        }

        public async Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Restaurant> restaurant)
        {
            return await _provinceService.GetRestaurantsDeliveryInforAsync(restaurant);
        }
        public async Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery)
        {
            return await _provinceService.GetFoodCataloguesAsync(delivery);
        }

        public async Task<List<RestaurantCategory>> GetMetadataForCategoryAsync()
        {
            return await _provinceService.GetMetadataForCategoryAsync();
        }
    }
}
