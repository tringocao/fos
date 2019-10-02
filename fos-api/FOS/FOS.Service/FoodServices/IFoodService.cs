using FOS.Model.Domain.NowModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOS.Services.FoodServices
{
    public interface IFoodService
    {
        string GetExternalServiceById(int idService);
        Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery);
        Task<List<FoodCategory>> GetFoodCataloguesFromDeliveryIdAsync(int deliveryId);
        Task<List<Food>> GetFoodFromCatalogueAsync(int deliveryId, int dishTypeId);
        Task<List<int>> GetDiscountedFoodIds(int deliveryId);
    }
}