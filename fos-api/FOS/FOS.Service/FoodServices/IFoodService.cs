using FOS.Model.Dto;
using System.Collections.Generic;

namespace FOS.Services.FoodServices
{
    public interface IFoodService
    {
        string GetExternalServiceById(int IdService);
        List<FoodCatalogue> GetFoodCatalogues(DeliveryInfos delivery);
        List<FoodCatalogue> GetFoodCataloguesFromDeliveryId(int delivery_id);
        List<Food> GetFoodFromCatalogue(int delivery_id, int dish_type_id);
    }
}