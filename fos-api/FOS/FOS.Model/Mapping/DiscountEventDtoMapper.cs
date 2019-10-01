using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IDiscountEventDtoMapper
    {
        Model.Domain.DiscountEvent ToModel(Dto.DiscountEvent discountEvent);
        Dto.DiscountEvent ToDto(Model.Domain.DiscountEvent discountEvent);
    }

    public class DiscountEventDtoMapper : IDiscountEventDtoMapper
    {
        public Dto.DiscountEvent ToDto(Model.Domain.DiscountEvent discountEvent)
        {
            return new Dto.DiscountEvent()
            {
              Id = discountEvent.Id,
              EventId = discountEvent.EventId,
              Discounts = discountEvent.Discounts
            };
        }

        public Model.Domain.DiscountEvent ToModel(Dto.DiscountEvent discountEvent)
        {
            return new Domain.DiscountEvent()
            {
                Id = discountEvent.Id,
                EventId = discountEvent.EventId,
                Discounts = discountEvent.Discounts
            };
        }
    }
}
