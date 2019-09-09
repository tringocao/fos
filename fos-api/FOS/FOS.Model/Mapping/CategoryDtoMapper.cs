using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;
using FOS.Model.Domain.NowModel;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public class CategoryDtoMapper : ICategoryDtoMapper
    {
        public Category ToDto(RestaurantCategory restaurantCategory)
        {
            return new Category()
            {
                Code = restaurantCategory.Code,
                Id = restaurantCategory.Id,
                Name = restaurantCategory.Name
            };
        }

        public RestaurantCategory ToModel(Category category)
        {
            return new RestaurantCategory()
            {
                Code = category.Code,
                Id = category.Id,
                Name = category.Name
            };
        }
    }

    public interface ICategoryDtoMapper
    {
        Dto.Category ToDto(Model.Domain.NowModel.RestaurantCategory restaurantCategory);
        Model.Domain.NowModel.RestaurantCategory ToModel(Dto.Category category);

    }
}
