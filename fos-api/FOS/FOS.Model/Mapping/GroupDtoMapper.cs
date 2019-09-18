using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public class GroupDtoMapper : IGroupDtoMapper
    {
        public Dto.Group ToDto(Domain.Group group)
        {
            return new Dto.Group()
            {
               Id = group.Id,
               DisplayName = group.DisplayName,
               Mail = group.Mail
            };
        }

        public Domain.Group ToDomain(Dto.Group dtoGroup)
        {
            return new Domain.Group()
            {
                Id = dtoGroup.Id,
                DisplayName = dtoGroup.DisplayName,
                Mail = dtoGroup.Mail
            };
        }
    }

    public interface IGroupDtoMapper
    {
        Dto.Group ToDto(Model.Domain.Group domainGroup);
        Domain.Group ToDomain(Model.Dto.Group dtoGroup);
    }
}
