using System.Collections.Generic;

namespace FOS.Services.EventPromotionServices
{
    public interface IEventPromotionService
    {
        IEnumerable<Model.Domain.EventPromotion> GetAllEventPromotions();
        Model.Domain.EventPromotion GetById(int id);
        Model.Domain.EventPromotion GetByEventId(int eventId);

        bool UpdateEventPromotion(Model.Domain.EventPromotion eventPromotion);
        bool AddEventPromotion(Model.Domain.EventPromotion eventPromotion);
        bool UpdateEventPromotionByEventId(int eventId, List<Model.Domain.NowModel.Promotion> newPromos);
        bool DeleteById(int id);
    }
}