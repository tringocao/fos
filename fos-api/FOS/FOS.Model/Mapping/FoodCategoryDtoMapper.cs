using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public class FoodCategoryDtoMapper : IFoodCategoryDtoMapper
    {
        IFoodDtoMapper _foodDtoMapper;
        FoodCategoryDtoMapper(IFoodDtoMapper foodDtoMapper)
        {
            _foodDtoMapper = foodDtoMapper;
        }
        public Dto.FoodCategory ToDto(Domain.NowModel.FoodCategory foodCategory)
        {
            return new Dto.FoodCategory()
            {
                DishTypeId = foodCategory.DishTypeId,
                DishTypeName = foodCategory.DishTypeName,
                Dishes = foodCategory.Dishes.Select(f => _foodDtoMapper.ToDto(f)).ToList()
            };
        }
    }

    public interface IFoodCategoryDtoMapper
    {
        Dto.FoodCategory ToDto(Model.Domain.NowModel.FoodCategory foodCategory);

    }
}
