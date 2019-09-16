using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IGraphUserDtoMapper
    {
        Dto.GraphUser ToDto(Model.Domain.User user);
    }

    public class GraphUserDtoMapper : IGraphUserDtoMapper
    { 
        public Dto.GraphUser ToDto(Model.Domain.User user)
        {
            return new Dto.GraphUser()
            {
                DisplayName = user.DisplayName,
                Id = user.Id,
                Mail = user.Mail,
                UserPrincipalName = user.UserPrincipalName
            };
        }
    }
}
