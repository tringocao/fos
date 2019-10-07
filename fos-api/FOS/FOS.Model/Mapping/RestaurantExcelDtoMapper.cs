using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IRestaurantExcelDtoMapper
    {
        Model.Domain.RestaurantExcel ToDomain(Model.Dto.RestaurantExcel dto);
        Model.Dto.RestaurantExcel ToDto(Model.Domain.RestaurantExcel domain);
    }
    public class RestaurantExcelDtoMapper: IRestaurantExcelDtoMapper
    {
        public Model.Domain.RestaurantExcel ToDomain(Model.Dto.RestaurantExcel dto)
        {
            return new Model.Domain.RestaurantExcel
            {
                Name = dto.Name,
                Address = dto.Address
            };
        }
        public Model.Dto.RestaurantExcel ToDto(Model.Domain.RestaurantExcel domain)
        {
            return new Model.Dto.RestaurantExcel
            {
                Name = domain.Name,
                Address = domain.Address
            };
        }
    }
}
