using FOS.Model.Domain.NowModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOS.Services.FoodServices
{
    public interface IFoodService
    {
        string GetExternalServiceById(int IdService);
        Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery);
        Task<List<FoodCategory>> GetFoodCataloguesFromDeliveryIdAsync(int delivery_id);
        Task<List<Food>> GetFoodFromCatalogueAsync(int delivery_id, int dish_type_id);
    }
}