using FOS.Repositories.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using FOS.Repositories.Repositories;
using System.Threading.Tasks;
using FOS.Model.Domain;

namespace FOS.Services.EventPromotionServices
{
    public class EventPromotionService: IEventPromotionService
    {
        IEventPromotionRepository _discountEventRepository;
        IEventPromotionMapper _discountEventMapper;
        public EventPromotionService(IEventPromotionRepository discountEventRepository, IEventPromotionMapper discountEventMapper)
        {
            _discountEventMapper = discountEventMapper;
            _discountEventRepository = discountEventRepository;
        }


        public bool AddDiscountEvent(EventPromotion discountEvent)
        {
            Repositories.DataModel.EventPromotion temp = new Repositories.DataModel.EventPromotion();
            _discountEventMapper.MapToEfObject(temp, discountEvent);
            return _discountEventRepository.AddDiscountEvent(temp);
        }

        public bool DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventPromotion> GetAllDiscountEvents()
        {
            return _discountEventRepository.GetAllDiscountEvents().Select(r => _discountEventMapper.MapToDomain(r));
        }

        public EventPromotion GetByEventId(int eventId)
        {
            return _discountEventMapper.MapToDomain(_discountEventRepository.GetByEventId(eventId));
        }

        public EventPromotion GetById(int id)
        {
            return _discountEventMapper.MapToDomain(_discountEventRepository.GetById(id));
        }

        public bool UpdateDiscountEvent(EventPromotion discountEvent)
        {
            Repositories.DataModel.EventPromotion temp = new Repositories.DataModel.EventPromotion();
            _discountEventMapper.MapToEfObject(temp, discountEvent);
            return _discountEventRepository.UpdateDiscountEvent(temp);
        }
    }
}
