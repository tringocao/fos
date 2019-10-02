using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;
using FOS.Model.Dto;

namespace FOS.Model.Mapping
{
    public interface ICustomGroupDtoMapper
    {
        Model.Dto.CustomGroup DomainToDto(Model.Domain.CustomGroup domain);
        Model.Domain.CustomGroup DtoToDomain(Model.Dto.CustomGroup dto);
    }
    public class CustomGroupDtoMapper : ICustomGroupDtoMapper
    {
        IGraphUserDtoMapper _grapUserMapper;
        public CustomGroupDtoMapper(IGraphUserDtoMapper graphUserMapper)
        {
            _grapUserMapper = graphUserMapper;
        }
        public Dto.CustomGroup DomainToDto(Domain.CustomGroup domain)
        {
            Model.Dto.CustomGroup customGroup = new Model.Dto.CustomGroup();
            customGroup.ID = domain.ID;
            customGroup.Name = domain.Name;
            customGroup.Owner = domain.Owner;
            customGroup.Users = domain.Users.Select(c => _grapUserMapper.GraphUserDomainToDto(c)).ToList();
            return customGroup;
        }
        public Domain.CustomGroup DtoToDomain(Dto.CustomGroup dto)
        {
            Model.Domain.CustomGroup customGroup = new Model.Domain.CustomGroup();
            customGroup.ID = dto.ID;
            customGroup.Name = dto.Name;
            customGroup.Owner = dto.Owner;
            customGroup.Users = dto.Users.Select(c => _grapUserMapper.GraphUserDtoToDomain(c)).ToList();
            return customGroup;
        }
    }
}
