using FOS.Model.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FOS.CoreService.RemindEventServices
{
    public interface IRemindEventServiceWS
    {
        IEnumerable<RecurrenceEvent> GetAllRecurranceEvents();
        bool UpdateRecurrenceEvent(Model.Domain.RecurrenceEvent recurrenceEvent);
        RecurrenceEvent GetRecurranceEventById(int id);
        Task<User> GetUserByIdAsync(string Id);
        string Parse<T>(string text, T modelparse);
        void checkRemindedTask();
    }
}