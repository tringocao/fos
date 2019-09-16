using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model;
using FOS.Repositories.DataModel;
using Newtonsoft.Json;

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
            return new Model.Domain.Order()
            {
                Id = Guid.Parse(efObject.Id),
                IdDelivery = efObject.IdDelivery,
                IdEvent = efObject.IdEvent,
                IdRestaurant = efObject.IdRestaurant,
                IdUser = efObject.IdUser,
                OrderDate = DateTime.Parse(efObject.OrderDate),
                FoodDetail = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<string, string>>>(efObject.FoodDetail),
                Email = efObject.Email
            };
        }

        public void MapToEfObject(DataModel.Order efObject, Model.Domain.Order domObject)
        {
            efObject.Id = domObject.Id.ToString();
            efObject.IdDelivery = domObject.IdDelivery;
            efObject.IdEvent = domObject.IdEvent;
            efObject.IdRestaurant = domObject.IdRestaurant;
            efObject.IdUser = domObject.IdUser;
            efObject.OrderDate = domObject.OrderDate.ToString();
            efObject.FoodDetail = domObject.FoodDetail != null ? JsonConvert.SerializeObject(domObject.FoodDetail) : "";
            efObject.Email = domObject.Email;
        }
    }
}
