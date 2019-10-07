using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IFoodReportDtoMapper
    {
        Model.Domain.FoodReport ToDomain(Dto.FoodReport dto);
        Model.Dto.FoodReport ToDto(Domain.FoodReport domain);
    }
    public class FoodReportDtoMapper: IFoodReportDtoMapper
    {
        public Domain.FoodReport ToDomain(Dto.FoodReport dto)
        {
            CommentsDtoMapper dtoMapper = new CommentsDtoMapper();
            return new Domain.FoodReport
            {
                Name = dto.Name,
                Price = dto.Price,
                TotalComment = dto.TotalComment,
                NumberOfUser = dto.NumberOfUser,
                Amount = dto.Amount,
                Picture = dto.Picture,
                FoodId = dto.FoodId,
                Total = dto.Total,
                UserIds = dto.UserIds,
                Comments = dto.Comments.Select(c => dtoMapper.ToDomain(c)).ToList(),
            };
        }
        public Dto.FoodReport ToDto(Domain.FoodReport domain)
        {
            CommentsDtoMapper dtoMapper = new CommentsDtoMapper();
            return new Dto.FoodReport
            {
                Name = domain.Name,
                Price = domain.Price,
                TotalComment = domain.TotalComment,
                NumberOfUser = domain.NumberOfUser,
                Amount = domain.Amount,
                Picture = domain.Picture,
                FoodId = domain.FoodId,
                Total = domain.Total,
                UserIds = domain.UserIds,
                Comments = domain.Comments.Select(c => dtoMapper.ToDto(c)).ToList(),
            };
        }
    }
}
