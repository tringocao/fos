using FOS.Model.Domain.NowModel;
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
        int idService;
        List<DeliveryInfos> deliveryInfos;
        public DeliveryService(IRestaurantService restaurantService)
        {
            //_craw = craw;
            _restaurantService = restaurantService;
        }
        public string GetExternalServiceById(int idService)
        {
            this.idService = idService;
            return "DeliveryService in " + _restaurantService.GetExternalServiceById(idService) + "is ready";
        }

        public async Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(int cityId, int restaurant_id)
        {
            return await _restaurantService.GetRestaurantDeliveryInforAsync(await
                _restaurantService.GetRestaurantsByIdAsync(cityId, restaurant_id));
        }
        public async Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(int cityId, List<Restaurant> restaurant_ids)
        {
            deliveryInfos = await _restaurantService.GetRestaurantsDeliveryInforAsync(restaurant_ids);
            return deliveryInfos;
        }
        public async Task<DeliveryInfos> GetRestaurantFirstDeliveryInforAsync(int cityId, int restaurant_id)
        {
            return (await GetRestaurantDeliveryInforAsync(cityId, restaurant_id)).FirstOrDefault();
        }
        public async Task<List<DeliveryInfos>> GetRestaurantDeliveryInforByPagingAsync(int cityId, int pagenum, int pageSize)
        {
            var listRestaurant = await _restaurantService.GetRestaurantsByProvinceAsync(cityId);
            var result = listRestaurant.Skip((pagenum - 1) * pageSize).Take(pageSize);
            return await GetRestaurantsDeliveryInforAsync(cityId, result.ToList());
        }

        public async Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery)
        {
            return await _restaurantService.GetFoodCataloguesAsync(delivery);
        }
        public async Task<List<DeliveryInfos>> SearchByNameAsync(string nameDelivery)
        {
            return deliveryInfos.FindAll(d => d.Name.Contains(nameDelivery));            
        }

        public async Task<DeliveryDetail> GetDetailAsync(int deliveryId)
        {
            return await _restaurantService.GetDeliveryDetailAsync(new Restaurant() { DeliveryId = deliveryId.ToString() });
        }
    }
}
