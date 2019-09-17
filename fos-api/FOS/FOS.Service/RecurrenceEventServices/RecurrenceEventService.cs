using FOS.Model.Domain;
using FOS.Repositories.Mapping;
using FOS.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.RecurrenceEventServices
{
    public class RecurrenceEventService: IRecurrenceEventService
    {
        IRecurrenceEventRepository _eventRepository;
        IRecurrenceEventMapper _recurrenceEventMapper;
        public RecurrenceEventService(IRecurrenceEventRepository eventRepository, IRecurrenceEventMapper recurrenceEventMapper)
        {
            _eventRepository = eventRepository;
            _recurrenceEventMapper = recurrenceEventMapper;
        }

        public bool AddRecurrenceEvent(RecurrenceEvent recurrenceEvent)
        {
            Repositories.DataModel.RecurrenceEvent temp = new Repositories.DataModel.RecurrenceEvent();
            _recurrenceEventMapper.MapToEfObject(temp, recurrenceEvent);
            return _eventRepository.AddRecurrenceEvent(temp);
        }

        public IEnumerable<RecurrenceEvent> GetAllRecurrenceEvents()
        {
            return _eventRepository.GetAllRecurrenceEvents().Select(r => _recurrenceEventMapper.MapToDomain(r));
        }

        public RecurrenceEvent GetById(int id)
        {
            return _recurrenceEventMapper.MapToDomain(_eventRepository.GetById(id));
        }
        public RecurrenceEvent GetByUserId(string userId)
        {
            return _recurrenceEventMapper.MapToDomain(_eventRepository.GetByUserId(userId));
        }
        public  bool DeleteById(int id)
        {
            return _eventRepository.DeleteRecurrenceEvent(id);
        }
        public bool UpdateRecurrenceEvent(RecurrenceEvent recurrenceEvent)
        {
            Repositories.DataModel.RecurrenceEvent temp = new Repositories.DataModel.RecurrenceEvent();
            _recurrenceEventMapper.MapToEfObject(temp, recurrenceEvent);
            return _eventRepository.UpdateRecurrenceEvent(temp);
        }
    }
}
