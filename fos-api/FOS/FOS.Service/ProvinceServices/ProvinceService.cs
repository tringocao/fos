using FOS.Model.Dto;
using FOS.Services.FoodServices;
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
        public string GetFoodServiceById(int IdService)
        {
            this.IdService = IdService;
            return "ProvinceService in " + _craw.GetFoodServiceById(IdService) + "is ready";
        }

        public List<Province> GetMetadata()
        {
           return _craw.GetMetadata();
        }

        public Province GetMetadataById(int city_id)
        {
            var listProvinces = GetMetadata();
            return listProvinces.Where(p => p.id == city_id.ToString()).FirstOrDefault();
        }

        public List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant)
        {
            return _craw.GetRestaurantDeliveryInfor(restaurant);
        }

        public List<Restaurant> GetRestaurants(Province province)
        {
            return _craw.GetRestaurants(province);
        }

        public List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant)
        {
            return _craw.GetRestaurantsDeliveryInfor(restaurant);
        }
    }
}
