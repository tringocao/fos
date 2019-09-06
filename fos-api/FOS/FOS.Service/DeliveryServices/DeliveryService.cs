using FOS.Model.Dto;
using FOS.Services.FoodServices;
using FOS.Services.RestaurantServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.DeliveryServices
{
    public class DeliveryService : IDeliveryService
    {
        IRestaurantService _restaurantService;
        int IdService;
        List<DeliveryInfos> deliveryInfos;
        public DeliveryService(IRestaurantService restaurantService)
        {
            //_craw = craw;
            _restaurantService = restaurantService;
        }
        public string GetExternalServiceById(int IdService)
        {
            this.IdService = IdService;
            return "DeliveryService in " + _restaurantService.GetExternalServiceById(IdService) + "is ready";
        }

        public async Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(int city_id, int restaurant_id)
        {
            return await _restaurantService.GetRestaurantDeliveryInforAsync(await
                _restaurantService.GetRestaurantsByIdAsync(city_id, restaurant_id));
        }
        public async Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(int city_id, List<Restaurant> restaurant_ids)
        {
            deliveryInfos = await _restaurantService.GetRestaurantsDeliveryInforAsync(restaurant_ids);
            return deliveryInfos;
        }
        public async Task<DeliveryInfos> GetRestaurantFirstDeliveryInforAsync(int city_id, int restaurant_id)
        {
            return (await GetRestaurantDeliveryInforAsync(city_id, restaurant_id)).FirstOrDefault();
        }
        public async Task<List<DeliveryInfos>> GetRestaurantDeliveryInforByPagingAsync(int city_id, int pagenum, int pageSize)
        {
            var listRestaurant = await _restaurantService.GetRestaurantsByProvinceAsync(city_id);
            var result = listRestaurant.Skip((pagenum - 1) * pageSize).Take(pageSize);
            return await GetRestaurantsDeliveryInforAsync(city_id, result.ToList());
        }

        public async Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery)
        {
            return await _restaurantService.GetFoodCataloguesAsync(delivery);
        }
        public async Task<List<DeliveryInfos>> SearchByNameAsync(string nameDelivery)
        {
            return deliveryInfos.FindAll(d => d.Name.Contains(nameDelivery));            
        }

        public async Task<DeliveryDetail> GetRestaurantDetailAsync(Restaurant restaurant)
        {
            return await _restaurantService.GetRestaurantDetailAsync(restaurant);
        }
    }
}
