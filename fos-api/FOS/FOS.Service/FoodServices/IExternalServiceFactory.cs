using FOS.Model.Domain;

namespace FOS.Services.FoodServices
{
    public interface IExternalServiceFactory
    {
        string Service(int id);
        IFoodService GetFoodService(APIsDTO api);
    }
}