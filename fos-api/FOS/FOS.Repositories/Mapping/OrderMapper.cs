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
        Model.Order MapToDomain(DataModel.Order efObject);
        void MapToEfObject(DataModel.Order efObject, Model.Order domObject);
    }

    public class OrderMapper : IOrderMapper
    {
        public Model.Order MapToDomain(DataModel.Order efObject)
        {
            throw new NotImplementedException();
        }

        public void MapToEfObject(DataModel.Order efObject, Model.Order domObject)
        {
            throw new NotImplementedException();
        }
    }
}
