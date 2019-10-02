using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Mapping
{
    public interface ICustomGroupMapper
    {
        Model.Domain.CustomGroup DataModelToDomain(DataModel.CustomGroup dataModel);
        DataModel.CustomGroup DomainToDataModel(Model.Domain.CustomGroup domain);
    }
    public class CustomGroupMapper: ICustomGroupMapper
    {
        IGraphUserMapper _grapUserMapper;
        public CustomGroupMapper(IGraphUserMapper graphUserMapper)
        {
            _grapUserMapper = graphUserMapper;
        }
        public Model.Domain.CustomGroup DataModelToDomain(DataModel.CustomGroup dataModel)
        {
            Model.Domain.CustomGroup customGroup = new Model.Domain.CustomGroup();
            customGroup.ID = dataModel.ID;
            customGroup.Name = dataModel.Name;
            customGroup.Owner = dataModel.Owner;
            customGroup.Users = _grapUserMapper.ListModelToDomain(dataModel.Users);
            return customGroup;
        }
        public DataModel.CustomGroup DomainToDataModel(Model.Domain.CustomGroup domain)
        {
            DataModel.CustomGroup customGroup = new DataModel.CustomGroup();
            customGroup.ID = domain.ID;
            customGroup.Name = domain.Name;
            customGroup.Owner = domain.Owner;
            customGroup.Users = _grapUserMapper.ListDomainToModel(domain.Users);
            return customGroup;
        }
    }
}
