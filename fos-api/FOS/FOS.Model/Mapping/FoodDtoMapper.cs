using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public class FoodDtoMapper : IFoodDtoMapper
    {
        public Dto.Food ToDto(Domain.NowModel.Food food)
        {
            return new Dto.Food()
            {
                Description = food.Description,
                Id = food.Id.ToString(),
                Name = food.Name,
                Photos = food.Photos.FirstOrDefault().Value,
                Price = food.Price.Text
            };
        }
    }

    public interface IFoodDtoMapper
    {
        Dto.Food ToDto(Model.Domain.NowModel.Food food);

    }
}
