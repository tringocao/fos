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
        public bool DeleteRecurrenceEvent(int id)
        {
            try
            {
                DataModel.RecurrenceEvent deleteRec =  _context.RecurrenceEvents.Find(id);
                _context.RecurrenceEvents.Remove(deleteRec);
                _context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
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
                DataModel.RecurrenceEvent updateRec = _context.RecurrenceEvents.FirstOrDefault(o => o.Id == recurrenceEvent.Id);
                {
                    _context.Entry(updateRec).CurrentValues.SetValues(recurrenceEvent);
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
        public DataModel.RecurrenceEvent GetByUserId(string Userid)
        {
            DataModel.RecurrenceEvent recurrenceEventByUserId = _context.RecurrenceEvents.FirstOrDefault(o => o.UserId == Userid);
            if (recurrenceEventByUserId == null) return new DataModel.RecurrenceEvent() {
                EndDate = DateTime.Now.ToString(),
                StartDate = DateTime.Now.ToString(),
                UserId = Userid,
                Title ="Unknown",
                TypeRepeat = "Daily",
            };
            else return recurrenceEventByUserId;
        }
    }

    public interface IRecurrenceEventRepository
    {
        DataModel.RecurrenceEvent GetByUserId(string Userid);
        bool DeleteRecurrenceEvent(int id);
        IEnumerable<DataModel.RecurrenceEvent> GetAllRecurrenceEvents();
        DataModel.RecurrenceEvent GetById(int id);
        bool UpdateRecurrenceEvent(DataModel.RecurrenceEvent recurrenceEvent);
        bool AddRecurrenceEvent(DataModel.RecurrenceEvent recurrenceEvent);
    }
}
