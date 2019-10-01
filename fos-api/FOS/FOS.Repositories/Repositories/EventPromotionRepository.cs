using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public class EventPromotionRepository : IEventPromotionRepository
    {
        private readonly FosContext _context;
        public EventPromotionRepository (FosContext context)
        {
            _context = context;
        }
        public bool DeleteDiscountEventByEventId(int idEvent)
        {
            try
            {
                DataModel.EventPromotion delete = _context.EventPromotions.FirstOrDefault(o => o.EventId == idEvent);
                if (delete == null) return true;
                _context.EventPromotions.Remove(delete);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataModel.EventPromotion GetByEventId(int idEvent)
        {
            try
            {
                return _context.EventPromotions.FirstOrDefault(o => o.EventId == idEvent);

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool AddDiscountEvent(DataModel.EventPromotion discountEvent)
        {
            try
            {
                _context.EventPromotions.Add(discountEvent);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateDiscountEvent(DataModel.EventPromotion discountEvent)
        {
            try
            {
                DataModel.EventPromotion update = _context.EventPromotions.FirstOrDefault(o => o.EventId == discountEvent.EventId);
                {
                    _context.Entry(update).CurrentValues.SetValues(update);
                    _context.SaveChanges();
                    return true;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataModel.EventPromotion GetById(int id)
        {
            return _context.EventPromotions.Find(id);
        }
        public IEnumerable<DataModel.EventPromotion> GetAllDiscountEvents()
        {
            return _context.EventPromotions;
        }
    }

    public interface IEventPromotionRepository
    {
        bool DeleteDiscountEventByEventId(int idEvent);
        bool AddDiscountEvent(DataModel.EventPromotion discountEvent);
        bool UpdateDiscountEvent(DataModel.EventPromotion discountEvent);
        DataModel.EventPromotion GetById(int id);
        DataModel.EventPromotion GetByEventId(int idEvent);

        IEnumerable<DataModel.EventPromotion> GetAllDiscountEvents();
    }
}
