using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;

namespace FOS.Services.ExternalServices.GrabFoodService
{
    public class GrabFoodService : IExternalService
    {
        public Task<List<Model.Domain.NowModel.FoodCategory>> GetFoodCataloguesAsync(Model.Domain.NowModel.DeliveryInfos delivery)
        {
            throw new NotImplementedException();
        }

        public Task<List<Model.Domain.NowModel.FoodCategory>> GetFoodFromDelivery(List<Model.Domain.NowModel.DeliveryInfos> deliveries)
        {
            throw new NotImplementedException();
        }

        public Task<List<RestaurantCategory>> GetMetadataForCategoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Model.Domain.NowModel.Province>> GetMetadataForProvinceAsync()
        {
            throw new NotImplementedException();
        }

        public string GetNameService()
        {
            throw new NotImplementedException();
        }

        public Task<List<Model.Domain.NowModel.DeliveryInfos>> GetRestaurantDeliveryInforAsync(Model.Domain.NowModel.Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public Task<DeliveryDetail> GetRestaurantDetailAsync(Model.Domain.NowModel.Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public Task<List<Model.Domain.NowModel.Restaurant>> GetRestaurantsAsync(Model.Domain.NowModel.Province province, string keyword, List<RestaurantCategory> category)
        {
            throw new NotImplementedException();
        }

        public Task<List<Model.Domain.NowModel.DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Model.Domain.NowModel.Restaurant> restaurant)
        {
            throw new NotImplementedException();
        }
    }
}
