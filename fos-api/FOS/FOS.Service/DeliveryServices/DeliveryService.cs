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

        public List<DeliveryInfos> GetRestaurantDeliveryInfor(int city_id, int restaurant_id)
        {
            return _restaurantService.GetRestaurantDeliveryInfor(
                _restaurantService.GetRestaurantsById(city_id, restaurant_id));
        }
        public List<DeliveryInfos> GetRestaurantsDeliveryInfor(int city_id, List<Restaurant> restaurant_ids)
        {
            deliveryInfos = _restaurantService.GetRestaurantsDeliveryInfor(restaurant_ids);
            return deliveryInfos;
        }
        public DeliveryInfos GetRestaurantFirstDeliveryInfor(int city_id, int restaurant_id)
        {
            return GetRestaurantDeliveryInfor(city_id, restaurant_id).FirstOrDefault();
        }
        public List<DeliveryInfos> GetRestaurantDeliveryInforByPaging(int city_id, int pagenum, int pageSize)
        {
            var listRestaurant = _restaurantService.GetRestaurantsByProvince(city_id);
            var result = listRestaurant.Skip((pagenum - 1) * pageSize).Take(pageSize);         
            return GetRestaurantsDeliveryInfor(city_id, result.ToList());
        }

        public List<FoodCatalogue> GetFoodCatalogues(DeliveryInfos delivery)
        {
            return _restaurantService.GetFoodCatalogues(delivery);
        }
        public List<DeliveryInfos> SearchByName(string nameDelivery)
        {
            return deliveryInfos.FindAll(d => d.name.Contains(nameDelivery));            
        }
    }
}
