using FOS.Model.Domain.NowModel;
using FOS.Services.DeliveryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.FoodServices
{
    public class FoodService:IFoodService
    {
        IDeliveryService _deliveryService;
        int IdService;
        public FoodService(IDeliveryService deliveryService)
        {
            //_craw = craw;
            _deliveryService = deliveryService;
        }
        public string GetExternalServiceById(int IdService)
        {
            this.IdService = IdService;
            return "DeliveryService in " + _deliveryService.GetExternalServiceById(IdService) + "is ready";
        }
        public async Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery)
        {
            return await _deliveryService.GetFoodCataloguesAsync(delivery);
        }
        public async Task<List<FoodCategory>> GetFoodCataloguesFromDeliveryIdAsync(int delivery_id)
        {
            return await GetFoodCataloguesAsync(
                new DeliveryInfos() { DeliveryId = delivery_id.ToString()});
        }
        public async Task<List<Food>> GetFoodFromCatalogueAsync(int delivery_id, int dish_type_id)
        {
            var listFoodCatalogue = await GetFoodCataloguesFromDeliveryIdAsync(delivery_id);
            return listFoodCatalogue
                .Where(fc => fc.DishTypeId == dish_type_id.ToString())
                .FirstOrDefault()
                .Dishes;
        }
    }
}
