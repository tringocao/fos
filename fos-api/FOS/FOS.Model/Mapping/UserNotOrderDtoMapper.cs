using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IUserNotOrderDtoMapper
    {
        Dto.UserNotOrder ToDomain(Domain.UserNotOrder user);
        IEnumerable<Dto.UserNotOrder> ListToDomain(IEnumerable<Domain.UserNotOrder> users);
    }
    public class UserNotOrderDtoMapper : IUserNotOrderDtoMapper
    {
        public Dto.UserNotOrder ToDomain(Domain.UserNotOrder user)
        {
            return new Dto.UserNotOrder()
            {
                OrderId = user.OrderId,
                UserId = user.UserId
            };
        }
        public IEnumerable<Dto.UserNotOrder> ListToDomain(IEnumerable<Domain.UserNotOrder> users)
        {
            return users.Select(c => ToDomain(c)).ToList();
        }
    }
}
