using FOS.Model.Dto;
using System.Collections.Generic;

namespace FOS.Services.FoodServices
{
    public interface IFoodService
    {
        string GetExternalServiceById(int IdService);
        List<FoodCategory> GetFoodCatalogues(DeliveryInfos delivery);
        List<FoodCategory> GetFoodCataloguesFromDeliveryId(int delivery_id);
        List<Food> GetFoodFromCatalogue(int delivery_id, int dish_type_id);
    }
}