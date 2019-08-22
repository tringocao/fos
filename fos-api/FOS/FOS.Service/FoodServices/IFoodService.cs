using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.FoodServices
{
    public interface IFoodService
    {
        string GetNameService();
        List<Restaurant> GetRestaurantsAsync();
        List<Food> GetFoods(Restaurant restaurant);

    }
}
