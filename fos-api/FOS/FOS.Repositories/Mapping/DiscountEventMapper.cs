
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Mapping
{
    public interface IDiscountEventMapper
    {
        Model.Domain.DiscountEvent MapToDomain(DataModel.EventPromotion efObject);
        void MapToEfObject(DataModel.EventPromotion efObject, Model.Domain.DiscountEvent domObject);
    }
    public class DiscountEventMapper : IDiscountEventMapper
    {
        public Model.Domain.DiscountEvent MapToDomain(DataModel.EventPromotion efObject)
        {
            return new Model.Domain.DiscountEvent()
            {
                Id = efObject.Id,
                EventId = efObject.EventId,
                Discounts = JsonConvert.DeserializeObject<Dictionary<string, float>>(efObject.Discounts),
            };
        }

        public void MapToEfObject(DataModel.EventPromotion efObject, Model.Domain.DiscountEvent domObject)
        {
            efObject.Id = domObject.Id;
            efObject.EventId = domObject.EventId;
            efObject.Discounts = domObject.Discounts != null ? JsonConvert.SerializeObject(domObject.Discounts) : "";
        }
    }
}
