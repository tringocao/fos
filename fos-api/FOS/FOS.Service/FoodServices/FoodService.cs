using FOS.Model.Dto;
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
        public List<FoodCatalogue> GetFoodCatalogues(DeliveryInfos delivery)
        {
            return _deliveryService.GetFoodCatalogues(delivery);
        }
        public List<FoodCatalogue> GetFoodCataloguesFromDeliveryId(int delivery_id)
        {          
            return GetFoodCatalogues(
                new DeliveryInfos() { delivery_id = delivery_id.ToString()});
        }
        public List<Food> GetFoodFromCatalogue(int delivery_id, int dish_type_id)
        {
            var listFoodCatalogue = GetFoodCataloguesFromDeliveryId(delivery_id);
            return listFoodCatalogue
                .Where(fc => fc.dish_type_id == dish_type_id.ToString())
                .FirstOrDefault()
                .dishes;
        }
    }
}
