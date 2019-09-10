using FOS.Model.Dto;
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
        IEnumerable<Event> GetAllEvent(string id);
        Event GetEvent(int id);
    }
}
