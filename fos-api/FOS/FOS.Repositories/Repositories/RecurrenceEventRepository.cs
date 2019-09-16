using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.Repositories
{
    public class RecurrenceEventRepository: IRecurrenceEventRepository
    {
        private readonly FosContext _context;
        public RecurrenceEventRepository(FosContext context)
        {
            _context = context;
        }

        public bool AddRecurrenceEvent(DataModel.RecurrenceEvent recurrenceEvent)
        {
            try
            {
                _context.RecurrenceEvents.Add(recurrenceEvent);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool UpdateRecurrenceEvent(DataModel.RecurrenceEvent recurrenceEvent)
        {
            try
            {
                DataModel.RecurrenceEvent updateOrder = _context.RecurrenceEvents.FirstOrDefault(o => o.Id == recurrenceEvent.Id);
                {
                    _context.Entry(updateOrder).CurrentValues.SetValues(recurrenceEvent);
                    _context.SaveChanges();
                    return true;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataModel.RecurrenceEvent GetById(int id)
        {
            return _context.RecurrenceEvents.Find(id);
        }
        public IEnumerable<DataModel.RecurrenceEvent> GetAllRecurrenceEvents()
        {
            return _context.RecurrenceEvents;
        }
    }

    public interface IRecurrenceEventRepository
    {
        IEnumerable<DataModel.RecurrenceEvent> GetAllRecurrenceEvents();
        DataModel.RecurrenceEvent GetById(int id);
        bool UpdateRecurrenceEvent(DataModel.RecurrenceEvent recurrenceEvent);
        bool AddRecurrenceEvent(DataModel.RecurrenceEvent recurrenceEvent);
    }
}
