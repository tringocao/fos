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
        Dto.GraphUser GraphUserDomainToDto(Model.Domain.GraphUser user);
        Domain.GraphUser GraphUserDtoToDomain(Model.Dto.GraphUser user);
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
        public Dto.GraphUser GraphUserDomainToDto(Model.Domain.GraphUser user)
        {
            return new Dto.GraphUser()
            {
                DisplayName = user.DisplayName,
                Id = user.Id,
                Mail = user.Mail,
                UserPrincipalName = user.UserPrincipalName
            };
        }
        public Domain.GraphUser GraphUserDtoToDomain(Model.Dto.GraphUser user)
        {
            return new Domain.GraphUser()
            {
                DisplayName = user.DisplayName,
                Id = user.Id,
                Mail = user.Mail,
                UserPrincipalName = user.UserPrincipalName
            };
        }
    }
}
