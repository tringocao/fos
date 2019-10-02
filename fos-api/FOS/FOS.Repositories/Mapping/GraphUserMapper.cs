using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Mapping
{
    public interface IGraphUserMapper
    {
        IEnumerable<Model.Domain.GraphUser> ListModelToDomain(ICollection<DataModel.GraphUserInGroup> dataModel);
        ICollection<DataModel.GraphUserInGroup> ListDomainToModel(IEnumerable<Model.Domain.GraphUser> domain);
        Model.Domain.GraphUser DataModelToDomain(DataModel.GraphUserInGroup dataModel);
        DataModel.GraphUserInGroup DomainToDataModel(Model.Domain.GraphUser domain);
    }
    public class GraphUserMapper: IGraphUserMapper
    {
        public Model.Domain.GraphUser DataModelToDomain(DataModel.GraphUserInGroup dataModel)
        {
            Model.Domain.GraphUser result = new Model.Domain.GraphUser();
            result.DisplayName = dataModel.DisplayName;
            result.Id = dataModel.UserId;
            result.Mail = dataModel.Mail;
            result.UserPrincipalName = dataModel.UserPrincipalName;
            return result;
        }
        public DataModel.GraphUserInGroup DomainToDataModel(Model.Domain.GraphUser domain)
        {
            DataModel.GraphUserInGroup result = new DataModel.GraphUserInGroup();
            result.DisplayName = domain.DisplayName;
            result.UserId = domain.Id;
            result.Mail = domain.Mail;
            result.UserPrincipalName = domain.UserPrincipalName;
            return result;
        }
        public IEnumerable<Model.Domain.GraphUser> ListModelToDomain(ICollection<DataModel.GraphUserInGroup> dataModel)
        {
            return dataModel.Select(c => DataModelToDomain(c)).ToList();
        }
        public ICollection<DataModel.GraphUserInGroup> ListDomainToModel(IEnumerable<Model.Domain.GraphUser> domain)
        {
            return domain.Select(c => DomainToDataModel(c)).ToList();
        }
    }
}
