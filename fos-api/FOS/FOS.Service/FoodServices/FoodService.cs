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
        int idService;
        public FoodService(IDeliveryService deliveryService)
        {
            //_craw = craw;
            _deliveryService = deliveryService;
        }
        public string GetExternalServiceById(int idService)
        {
            this.idService = idService;
            return "DeliveryService in " + _deliveryService.GetExternalServiceById(idService) + "is ready";
        }
        public async Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery)
        {
            return await _deliveryService.GetFoodCataloguesAsync(delivery);
        }
        public async Task<List<FoodCategory>> GetFoodCataloguesFromDeliveryIdAsync(int deliveryId)
        {
            return await GetFoodCataloguesAsync(
                new DeliveryInfos() { DeliveryId = deliveryId});
        }
        public async Task<List<int>> GetDiscountedFoodIds(int deliveryId)
        {
            List<int> DiscountedFoodIds = new List<int>();
            var menu =  await GetFoodCataloguesAsync(
                new DeliveryInfos() { DeliveryId = deliveryId });
            foreach (var dishType in menu)
            {
                foreach(var dish in dishType.Dishes)
                {
                    if(dish.DiscountPrice != null)
                    {
                        DiscountedFoodIds.Add(dish.Id);
                    }
                }
                
            }
            return DiscountedFoodIds;
        }
        public async Task<List<Food>> GetFoodFromCatalogueAsync(int deliveryId, int dishTypeId)
        {
            var listFoodCatalogue = await GetFoodCataloguesFromDeliveryIdAsync(deliveryId);
            return listFoodCatalogue
                .Where(fc => fc.DishTypeId == dishTypeId.ToString())
                .FirstOrDefault()
                .Dishes;
        }
    }
}
