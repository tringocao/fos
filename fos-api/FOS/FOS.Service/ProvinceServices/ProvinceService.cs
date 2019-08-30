using FOS.Model.Dto;
using FOS.Services.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.ProvinceServices
{
    public class ProvinceService : IProvinceService
    {
        IExternalServiceFactory _craw;
        int IdService;
        public ProvinceService(IExternalServiceFactory craw)
        {
            _craw = craw;
        }
        public string GetExternalServiceById(int IdService)
        {
            this.IdService = IdService;
            return "ProvinceService in " + _craw.GetExternalServiceById(IdService) + "is ready";
        }

        public async Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery)
        {
            return await _craw.GetFoodCataloguesAsync(delivery);
        }


        public async Task<List<Province>> GetMetadataForProvinceAsync()
        {
            return await _craw.GetMetadataForProvinceAsync();
        }

        public async Task<Province> GetMetadataByIdAsync(int city_id)
        {
            var listProvinces = await GetMetadataForProvinceAsync();
            return  listProvinces.Where(p => p.id == city_id.ToString()).FirstOrDefault();
        }

        public async Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(Restaurant restaurant)
        {
            return await _craw.GetRestaurantDeliveryInforAsync(restaurant);
        }

        public async Task<List<Restaurant>> GetRestaurantsAsync(Province province, string keyword, List<RestaurantCategory> categories)
        {
            return await _craw.GetRestaurantsAsync(province, keyword, categories);
        }

        public async Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Restaurant> restaurant)
        {
            return await _craw.GetRestaurantsDeliveryInforAsync(restaurant);
        }

        public async Task<List<RestaurantCategory>> GetMetadataForCategoryAsync()
        {
            return await _craw.GetMetadataForCategoryAsync();
        }
    }
}