using FOS.Model.Domain;
using FOS.Services.RecurrenceEventServices;
using FOS.Services.SendEmailServices;
using FOS.Services.SPUserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.CoreService.RemindEventServices
{
    public class RemindEventService
    {
        IRecurrenceEventService _recurrenceEventService;
        ISPUserService _iSPUserService;
        ISendEmailService _sendEmailService;
        public RemindEventService(IRecurrenceEventService recurrenceEventService, ISPUserService iSPUserService, ISendEmailService sendEmailService)
        {
            _recurrenceEventService = recurrenceEventService;
            _iSPUserService = iSPUserService;
            _sendEmailService = sendEmailService;
        }
        public IEnumerable<RecurrenceEvent> GetAllRecurranceEvents()
        {
            return _recurrenceEventService.GetAllRecurrenceEvents();
        }
        public bool UpdateRecurrenceEvent(Model.Domain.RecurrenceEvent recurrenceEvent)
        {
            return _recurrenceEventService.UpdateRecurrenceEvent(recurrenceEvent);
        }
        public RecurrenceEvent GetRecurranceEventById(int id)
        {
            return _recurrenceEventService.GetById(id);
        }
        public async Task<User> GetUserByIdAsync(string Id)
        {
            return await _iSPUserService.GetUserById(Id);
        }
        public string Parse<T>(string text, T modelparse)
        {
            return _sendEmailService.Parse(text, modelparse);
        }
    }
}
