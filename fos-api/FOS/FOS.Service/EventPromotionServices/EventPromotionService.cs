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
        IEventPromotionRepository _eventPromotionRepository;
        IEventPromotionMapper _eventPromotionMapper;
        public EventPromotionService(IEventPromotionRepository discountEventRepository, IEventPromotionMapper discountEventMapper)
        {
            _eventPromotionMapper = discountEventMapper;
            _eventPromotionRepository = discountEventRepository;
        }


        public bool AddEventPromotion(EventPromotion eventPromotion)
        {
            Repositories.DataModel.EventPromotion temp = new Repositories.DataModel.EventPromotion();
            _eventPromotionMapper.MapToEfObject(temp, eventPromotion);
            return _eventPromotionRepository.AddEventPromotion(temp);
        }

        public bool DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EventPromotion> GetAllEventPromotions()
        {
            return _eventPromotionRepository.GetAllEventPromotions().Select(r => _eventPromotionMapper.MapToDomain(r));
        }

        public EventPromotion GetByEventId(int eventId)
        {
            var eventPromotion = _eventPromotionRepository.GetByEventId(eventId);
            if (eventPromotion != null)
            {
                return _eventPromotionMapper.MapToDomain(eventPromotion);
            }
            return null;
        }

        public EventPromotion GetById(int id)
        {
            return _eventPromotionMapper.MapToDomain(_eventPromotionRepository.GetById(id));
        }

        public bool UpdateEventPromotion(EventPromotion eventPromotion)
        {
            Repositories.DataModel.EventPromotion temp = new Repositories.DataModel.EventPromotion();
            _eventPromotionMapper.MapToEfObject(temp, eventPromotion);
            return _eventPromotionRepository.UpdateEventPromotion(temp);
        }
        public bool UpdateEventPromotionByEventId(int eventId, List<Model.Domain.NowModel.Promotion> newPromos)
        {
            Repositories.DataModel.EventPromotion temp = new Repositories.DataModel.EventPromotion();
            var eventPromotion = GetByEventId(eventId);
            eventPromotion.Promotions.AddRange(newPromos);
            _eventPromotionMapper.MapToEfObject(temp, eventPromotion);
            return _eventPromotionRepository.UpdateEventPromotion(temp);
        }
    }
}
