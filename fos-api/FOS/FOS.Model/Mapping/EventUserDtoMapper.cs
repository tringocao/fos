using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IEventUserDtoMapper
    {
        Dto.EventUsers ToDto(Model.Domain.EventUsers user);
        Domain.EventUsers ToDomain(Dto.EventUsers dtoUser);
    }
    public class EventUserDtoMapper: IEventUserDtoMapper
    {
        public Dto.EventUsers ToDto(Domain.EventUsers user)
        {
            return new Dto.EventUsers()
            {
                EventId = user.EventId,
                EventTitle = user.EventTitle,
                UserMail = user.UserMail,
                UserName = user.UserName
            };
        }
        public Domain.EventUsers ToDomain(Dto.EventUsers user)
        {
            return new Domain.EventUsers()
            {
                EventId = user.EventId,
                EventTitle = user.EventTitle,
                UserMail = user.UserMail,
                UserName = user.UserName
            };
        }
    }
}
