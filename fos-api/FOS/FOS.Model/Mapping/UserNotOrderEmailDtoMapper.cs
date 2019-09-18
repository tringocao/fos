using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IUserNotOrderEmailDtoMapper
    {
        Model.Domain.UserNotOrderEmail ToModel(Dto.UserNotOrderEmail userNotOrderEmail);
        Dto.UserNotOrderEmail ToDto(Model.Domain.UserNotOrderEmail userNotOrderEmail);
    }

    public class UserNotOrderEmailDtoMapper : IUserNotOrderEmailDtoMapper
    {
        public  Dto.UserNotOrderEmail ToDto(Model.Domain.UserNotOrderEmail userNotOrderEmail)
        {
            return new Dto.UserNotOrderEmail()
            {
                UserEmail =userNotOrderEmail.UserEmail,
                OrderId = userNotOrderEmail.OrderId
            };
        }

        public Model.Domain.UserNotOrderEmail ToModel(Dto.UserNotOrderEmail userNotOrderEmail)
        {
            return new Domain.UserNotOrderEmail()
            {
                UserEmail = userNotOrderEmail.UserEmail,
                OrderId = userNotOrderEmail.OrderId
            };
        }
    }
}
