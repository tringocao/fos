using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public class NewGraphUserDtoMapper : INewGraphUserDtoMapper
    {
        public Dto.GraphUser ToDto(Model.Domain.GraphUser graphUser)
        {
            return new Dto.GraphUser()
            {
                Id = graphUser.Id,
                DisplayName = graphUser.DisplayName,
                Mail = graphUser.Mail,
                UserPrincipalName = graphUser.UserPrincipalName
            };
        }

        public Domain.GraphUser ToDomain(Dto.GraphUser dtoGraphUser)
        {
            return new Domain.GraphUser()
            {
               DisplayName = dtoGraphUser.DisplayName,
               Id = dtoGraphUser.Id,
               Mail = dtoGraphUser.Mail,
               UserPrincipalName = dtoGraphUser.UserPrincipalName
            };
        }
        public Domain.User ToDomainUser(Dto.GraphUser dtoGraphUser)
        {
            return new Domain.User()
            {
                DisplayName = dtoGraphUser.DisplayName,
                Id = dtoGraphUser.Id,
                Mail = dtoGraphUser.Mail,
                UserPrincipalName = dtoGraphUser.UserPrincipalName
            };
        }
    }

    public interface INewGraphUserDtoMapper
    {
        Dto.GraphUser ToDto(Model.Domain.GraphUser graphUser);
        Domain.GraphUser ToDomain(Dto.GraphUser dtoGraphUser);
        Domain.User ToDomainUser(Dto.GraphUser dtoGraphUser);
    }
}
