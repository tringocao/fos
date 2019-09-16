using System.Collections.Generic;

namespace FOS.Services.RecurrenceEventServices
{
    public interface IRecurrenceEventService
    {
        IEnumerable<Model.Domain.RecurrenceEvent> GetAllRecurrenceEvents();
        Model.Domain.RecurrenceEvent GetById(int id);
        bool UpdateRecurrenceEvent(Model.Domain.RecurrenceEvent recurrenceEvent);
        bool AddRecurrenceEvent(Model.Domain.RecurrenceEvent recurrenceEvent);
    }
}