using System.Collections.Generic;

namespace FOS.Services.EventPromotionServices
{
    public interface IEventPromotionService
    {
        IEnumerable<Model.Domain.EventPromotion> GetAllDiscountEvents();
        Model.Domain.EventPromotion GetById(int id);
        Model.Domain.EventPromotion GetByEventId(int eventId);

        bool UpdateDiscountEvent(Model.Domain.EventPromotion discountEvent);
        bool AddDiscountEvent(Model.Domain.EventPromotion discountEvent);
        bool DeleteById(int id);
    }
}