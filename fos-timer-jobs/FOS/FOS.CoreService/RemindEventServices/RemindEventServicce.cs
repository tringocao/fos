using FOS.Model.Domain;
using FOS.Services.RecurrenceEventServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.CoreService.RemindEventServices
{
    public class RemindEventServicce
    {
        IRecurrenceEventService _recurrenceEventService;
        public RemindEventServicce(IRecurrenceEventService recurrenceEventService)
        {
            _recurrenceEventService = recurrenceEventService;
        }
        public IEnumerable<RecurrenceEvent> GetAllRecurranceEvents()
        {
            return _recurrenceEventService.GetAllRecurrenceEvents();
        }
        public bool UpdateRecurrenceEvent(Model.Domain.RecurrenceEvent recurrenceEvent)
        {
            return _recurrenceEventService.UpdateRecurrenceEvent(recurrenceEvent);
        }
    }
}
