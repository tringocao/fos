using FOS.Model.Domain.NowModel;
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
        int idService;
        public RestaurantService(IProvinceService provinceService)
        {
            //_craw = craw;
            _provinceService = provinceService;
        }
        public string GetExternalServiceById(int idService)
        {
            this.idService = idService;
            return "RestaurantService in " + _provinceService.GetExternalServiceById(idService) + "is ready";
        }
        public async Task<List<Restaurant>> GetRestaurantsByProvinceAsync(int cityId)
        {
            return await _provinceService.GetRestaurantsAsync(await _provinceService.GetMetadataByIdAsync(cityId), "\"\"", null);
        }
        public async Task<List<Restaurant>> GetRestaurantsByKeywordAsync(int cityId, string keyword)
        {
            return await _provinceService.GetRestaurantsAsync(await _provinceService.GetMetadataByIdAsync(cityId), keyword, null);
        }
        public async Task<List<Restaurant>> GetRestaurantsByCategoriesAsync(int cityId, List<RestaurantCategory> categories)
        {

            return await _provinceService.GetRestaurantsAsync(await _provinceService.GetMetadataByIdAsync(cityId), "", categories);
        }
        public async Task<List<Restaurant>> GetRestaurantsByCategoriesKeywordAsync(int cityId, List<RestaurantCategory> categories, string keyword)
        {

            return await _provinceService.GetRestaurantsAsync(await _provinceService.GetMetadataByIdAsync(cityId), keyword, categories);
        }
        public async Task<Restaurant> GetRestaurantsByIdAsync(int cityId, int restaurant_id)
        {
            var listRestaurants = await GetRestaurantsByProvinceAsync(cityId);
            return listRestaurants.Where(p => p.RestaurantId == restaurant_id.ToString()).FirstOrDefault();
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

        public async Task<DeliveryDetail> GetDeliveryDetailAsync(Restaurant restaurant)
        {
            return await _provinceService.GetDeliveryDetailAsync(restaurant);
        }
    }
}
