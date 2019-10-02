using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public class DiscountEventRepository: IDiscountEventRepository
    {
        private readonly FosContext _context;
        public DiscountEventRepository(FosContext context)
        {
            _context = context;
        }
        public bool DeleteDiscountEventByEventId(int idEvent)
        {
            try
            {
                DataModel.DiscountEvent delete = _context.DiscountEvents.FirstOrDefault(o => o.EventId == idEvent);
                if (delete == null) return true;
                _context.DiscountEvents.Remove(delete);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataModel.DiscountEvent GetByEventId(int idEvent)
        {
            try
            {
                return _context.DiscountEvents.FirstOrDefault(o => o.EventId == idEvent);

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool AddDiscountEvent(DataModel.DiscountEvent discountEvent)
        {
            try
            {
                _context.DiscountEvents.Add(discountEvent);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateDiscountEvent(DataModel.DiscountEvent discountEvent)
        {
            try
            {
                DataModel.DiscountEvent update = _context.DiscountEvents.FirstOrDefault(o => o.EventId == discountEvent.EventId);
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
        public DataModel.DiscountEvent GetById(int id)
        {
            return _context.DiscountEvents.Find(id);
        }
        public IEnumerable<DataModel.DiscountEvent> GetAllDiscountEvents()
        {
            return _context.DiscountEvents;
        }
    }

    public interface IDiscountEventRepository
    {
        bool DeleteDiscountEventByEventId(int idEvent);
        bool AddDiscountEvent(DataModel.DiscountEvent discountEvent);
        bool UpdateDiscountEvent(DataModel.DiscountEvent discountEvent);
        DataModel.DiscountEvent GetById(int id);
        DataModel.DiscountEvent GetByEventId(int idEvent);

        IEnumerable<DataModel.DiscountEvent> GetAllDiscountEvents();
    }
}
