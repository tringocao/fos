using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public class CategoryGroupDtoMapper: ICategoryGroupDtoMapper
    {
        ICategoryDtoMapper _categoryDtoMapper;
        public CategoryGroupDtoMapper(ICategoryDtoMapper categoryDtoMapper)
        {
            _categoryDtoMapper = categoryDtoMapper;
        }
        public CategoryGroup ToDto(RestaurantCategory restaurantCategory)
        {

            return new CategoryGroup()
            {
                Id = restaurantCategory.Id,
                Name = restaurantCategory.Name,
                Code = restaurantCategory.Code,
                Categories = restaurantCategory.Categories.Select(c => _categoryDtoMapper.ToDto(c)).ToList()
            };
        }

        public RestaurantCategory ToModel(CategoryGroup categoryGroup)
        {
            return new RestaurantCategory()
            {
                Categories = categoryGroup.Categories.Select(c => _categoryDtoMapper.ToModel(c)).ToList(),
                Code = categoryGroup.Code,
                Name = categoryGroup.Name,
                Id = categoryGroup.Id
            };
        }
    }

    public interface ICategoryGroupDtoMapper
    {
        Dto.CategoryGroup ToDto(Model.Domain.NowModel.RestaurantCategory order);
        Model.Domain.NowModel.RestaurantCategory ToModel(Dto.CategoryGroup categoryGroup);

    }
}
