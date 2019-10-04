using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IExcelModelDtoMapper
    {
        Model.Domain.ExcelModel ToDomain(Dto.ExcelModel dto);
        Model.Dto.ExcelModel ToDto(Domain.ExcelModel domain);
    }
    public class ExcelModelDtoMapper: IExcelModelDtoMapper
    {
        public Domain.ExcelModel ToDomain(Dto.ExcelModel dto)
        {
            EventDtoMapper eventMapper = new EventDtoMapper();
            UserOrderDtoMapper userOrderMapper = new UserOrderDtoMapper();
            RestaurantExcelDtoMapper restaurantMapper = new RestaurantExcelDtoMapper();
            FoodReportDtoMapper foodMapper = new FoodReportDtoMapper();
            UserDtoMapper userDtoMapper = new UserDtoMapper();
            List<Model.Domain.UserOrder> listDomainUser = dto.UserOrder.Select(u => userOrderMapper.ToDomain(u)).ToList();
            List<Model.Domain.FoodReport> listFoodReport = dto.FoodReport.Select(f => foodMapper.ToDomain(f)).ToList();
            List<Model.Domain.User> listUser = dto.User.Select(u => userDtoMapper.ToDomain(u)).ToList();

            return new Domain.ExcelModel
            {
                Event = eventMapper.DtoToDomain(dto.Event),
                UserOrder = listDomainUser,
                RestaurantExcel = restaurantMapper.ToDomain(dto.RestaurantExcel),
                FoodReport = listFoodReport,
                User = listUser,
            };
        }
        public Dto.ExcelModel ToDto(Domain.ExcelModel domain)
        {
            EventDtoMapper eventMapper = new EventDtoMapper();
            UserOrderDtoMapper userOrderMapper = new UserOrderDtoMapper();
            RestaurantExcelDtoMapper restaurantMapper = new RestaurantExcelDtoMapper();
            FoodReportDtoMapper foodMapper = new FoodReportDtoMapper();
            UserDtoMapper userDtoMapper = new UserDtoMapper();
            List<Model.Dto.UserOrder> listDomainUser = domain.UserOrder.Select(u => userOrderMapper.ToDto(u)).ToList();
            List<Model.Dto.FoodReport> listFoodReport = domain.FoodReport.Select(f => foodMapper.ToDto(f)).ToList();
            List<Model.Dto.User> listUser = domain.User.Select(u => userDtoMapper.ToDto(u)).ToList();

            return new Dto.ExcelModel
            {
                Event = eventMapper.DomainToDto(domain.Event),
                UserOrder = listDomainUser,
                RestaurantExcel = restaurantMapper.ToDto(domain.RestaurantExcel),
                FoodReport = listFoodReport,
                User = listUser,
            };
        }
    }
}
