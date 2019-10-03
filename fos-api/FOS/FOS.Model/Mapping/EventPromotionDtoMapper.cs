using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Mapping
{
    public interface IEventPromotionDtoMapper
    {
        Model.Domain.EventPromotion ToModel(Dto.EventPromotion discountEvent);
        Dto.EventPromotion ToDto(Model.Domain.EventPromotion discountEvent);
    }

    public class EventPromotionDtoMapper : IEventPromotionDtoMapper
    {
        IPromotionDtoMapper _promotionDtoMapper;
        public EventPromotionDtoMapper(IPromotionDtoMapper promotionDtoMapper)
        {
            _promotionDtoMapper = promotionDtoMapper;
        }
        public Dto.EventPromotion ToDto(Model.Domain.EventPromotion discountEvent)
        {
            return new Dto.EventPromotion()
            {
                Id = discountEvent.Id,
                EventId = discountEvent.EventId,
                Promotions = discountEvent.Promotions.Select(p => _promotionDtoMapper.ToDto(p)).ToList()
            };
        }

        public Model.Domain.EventPromotion ToModel(Dto.EventPromotion discountEvent)
        {
            return new Domain.EventPromotion()
            {
                Id = discountEvent.Id,
                EventId = discountEvent.EventId,
                Promotions = discountEvent.Promotions.Select(p => _promotionDtoMapper.ToModel(p)).ToList()
            };
        }
    }
}
