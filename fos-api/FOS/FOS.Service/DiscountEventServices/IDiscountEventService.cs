using System.Collections.Generic;

namespace FOS.Services.DiscountEventServices
{
    public interface IDiscountEventService
    {
        IEnumerable<Model.Domain.DiscountEvent> GetAllDiscountEvents();
        Model.Domain.DiscountEvent GetById(int id);
        Model.Domain.DiscountEvent GetByEventId(int eventId);

        bool UpdateDiscountEvent(Model.Domain.DiscountEvent discountEvent);
        bool AddDiscountEvent(Model.Domain.DiscountEvent discountEvent);
        bool DeleteById(int id);
    }
}