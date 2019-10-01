using FOS.Repositories.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using FOS.Repositories.Repositories;
using System.Threading.Tasks;
using FOS.Model.Domain;

namespace FOS.Services.DiscountEventServices
{
    public class DiscountEventService: IDiscountEventService
    {
        IDiscountEventRepository _discountEventRepository;
        IDiscountEventMapper _discountEventMapper;
        public DiscountEventService(IDiscountEventRepository discountEventRepository, IDiscountEventMapper discountEventMapper)
        {
            _discountEventMapper = discountEventMapper;
            _discountEventRepository = discountEventRepository;
        }


        public bool AddDiscountEvent(DiscountEvent discountEvent)
        {
            Repositories.DataModel.EventPromotion temp = new Repositories.DataModel.EventPromotion();
            _discountEventMapper.MapToEfObject(temp, discountEvent);
            return _discountEventRepository.AddDiscountEvent(temp);
        }

        public bool DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DiscountEvent> GetAllDiscountEvents()
        {
            return _discountEventRepository.GetAllDiscountEvents().Select(r => _discountEventMapper.MapToDomain(r));
        }

        public DiscountEvent GetByEventId(int eventId)
        {
            return _discountEventMapper.MapToDomain(_discountEventRepository.GetByEventId(eventId));
        }

        public DiscountEvent GetById(int id)
        {
            return _discountEventMapper.MapToDomain(_discountEventRepository.GetById(id));
        }

        public bool UpdateDiscountEvent(DiscountEvent discountEvent)
        {
            Repositories.DataModel.EventPromotion temp = new Repositories.DataModel.EventPromotion();
            _discountEventMapper.MapToEfObject(temp, discountEvent);
            return _discountEventRepository.UpdateDiscountEvent(temp);
        }
    }
}
