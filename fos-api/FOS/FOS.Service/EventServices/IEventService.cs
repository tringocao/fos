using FOS.Services.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.EventServices
{
    public interface IEventService
    {
        Task<IEnumerable<EventModel>> GetAllEvent(string id);
        EventModel GetEvent(int id);
    }
}
