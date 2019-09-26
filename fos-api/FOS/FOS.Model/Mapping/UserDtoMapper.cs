using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public class UserDtoMapper : IUserDtoMapper
    {
        public Dto.User ToDto(Domain.User user)
        {
            return new Dto.User()
            {
                UserPrincipalName = user.UserPrincipalName,
                DisplayName = user.DisplayName,
                GivenName = user.GivenName,
                Id = user.Id,
                Surname = user.Surname,
                JobTitle = user.JobTitle,
                Mail = user.Mail,
                MobilePhone = user.MobilePhone,
                OfficeLocation = user.OfficeLocation,
                PreferredLanguage = user.PreferredLanguage
            };
        }
        public Domain.User ToDomain(Dto.User user)
        {
            return new Domain.User()
            {
                UserPrincipalName = user.UserPrincipalName,
                DisplayName = user.DisplayName,
                GivenName = user.GivenName,
                Id = user.Id,
                Surname = user.Surname,
                JobTitle = user.JobTitle,
                Mail = user.Mail,
                MobilePhone = user.MobilePhone,
                OfficeLocation = user.OfficeLocation,
                PreferredLanguage = user.PreferredLanguage
            };
        }
    }

    public interface IUserDtoMapper
    {
        Dto.User ToDto(Model.Domain.User user);
        Domain.User ToDomain(Dto.User dtoUser);
    }
}
