using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IUserOrderDtoMapper
    {
        Model.Domain.UserOrder ToDomain(Model.Dto.UserOrder dtoUser);
        Model.Dto.UserOrder ToDto(Model.Domain.UserOrder domainUser);
    }

    public class UserOrderDtoMapper : IUserOrderDtoMapper
    {
        public Model.Domain.UserOrder ToDomain(Model.Dto.UserOrder dtoUser)
        {
            UserDtoMapper mapper = new UserDtoMapper();
            CommentsDtoMapper dtoMapper = new CommentsDtoMapper();
            return new Domain.UserOrder()
            {
                User = mapper.ToDomain(dtoUser.User),
                Comments = dtoUser.Comments.Select(c => dtoMapper.ToDomain(c)).ToList(),
                Food = dtoUser.Food,
                PayExtra = dtoUser.PayExtra,
                Price = dtoUser.Price
            };
        }
        public Model.Dto.UserOrder ToDto(Model.Domain.UserOrder domainUser)
        {
            UserDtoMapper mapper = new UserDtoMapper();
            CommentsDtoMapper dtoMapper = new CommentsDtoMapper();
            return new Model.Dto.UserOrder()
            {
                User = mapper.ToDto(domainUser.User),
                Comments = domainUser.Comments.Select(c => dtoMapper.ToDto(c)).ToList(),
                Food = domainUser.Food,
                PayExtra = domainUser.PayExtra,
                Price = domainUser.Price
            };
        }
    }
}