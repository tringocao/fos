
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Mapping
{
    public interface IEventPromotionMapper
    {
        Model.Domain.EventPromotion MapToDomain(DataModel.EventPromotion efObject);
        void MapToEfObject(DataModel.EventPromotion efObject, Model.Domain.EventPromotion domObject);
    }
    public class EventPromotionMapper : IEventPromotionMapper
    {
        public Model.Domain.EventPromotion MapToDomain(DataModel.EventPromotion efObject)
        {
            return new Model.Domain.EventPromotion()
            {
                Id = efObject.Id,
                EventId = efObject.EventId,
                Promotions = JsonConvert.DeserializeObject<Dictionary<string, float>>(efObject.Promotions),
            };
        }

        public void MapToEfObject(DataModel.EventPromotion efObject, Model.Domain.EventPromotion domObject)
        {
            efObject.Id = domObject.Id;
            efObject.EventId = domObject.EventId;
            efObject.Promotions = domObject.Promotions != null ? JsonConvert.SerializeObject(domObject.Promotions) : "";
        }
    }
}
