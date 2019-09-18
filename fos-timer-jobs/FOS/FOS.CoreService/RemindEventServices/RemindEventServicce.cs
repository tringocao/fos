using FOS.Model.Domain;
using FOS.Services.RecurrenceEventServices;
using FOS.Services.SPUserService;
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
        ISPUserService _iSPUserService;
        public RemindEventServicce(IRecurrenceEventService recurrenceEventService, ISPUserService iSPUserService)
        {
            _recurrenceEventService = recurrenceEventService;
            _iSPUserService = iSPUserService;
        }
        public IEnumerable<RecurrenceEvent> GetAllRecurranceEvents()
        {
            return _recurrenceEventService.GetAllRecurrenceEvents();
        }
        public bool UpdateRecurrenceEvent(Model.Domain.RecurrenceEvent recurrenceEvent)
        {
            return _recurrenceEventService.UpdateRecurrenceEvent(recurrenceEvent);
        }
        public async Task<User> GetUserByIdAsync(string Id)
        {
            return await _iSPUserService.GetUserById(Id);
        }
    }
}
