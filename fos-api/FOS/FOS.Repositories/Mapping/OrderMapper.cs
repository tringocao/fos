using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model;
using FOS.Repositories.DataModel;

namespace FOS.Repositories.Mapping
{
    public interface IOrderMapper
    {
        Model.Domain.Order MapToDomain(DataModel.Order efObject);
        void MapToEfObject(DataModel.Order efObject, Model.Domain.Order domObject);
    }

    public class OrderMapper : IOrderMapper
    {
        public Model.Domain.Order MapToDomain(DataModel.Order efObject)
        {
            throw new NotImplementedException();
        }

        public void MapToEfObject(DataModel.Order efObject, Model.Domain.Order domObject)
        {
            throw new NotImplementedException();
        }
    }
}
