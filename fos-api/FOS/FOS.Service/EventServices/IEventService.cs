using FOS.Model.Dto;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;

namespace FOS.Services.EventServices
{
    public interface IEventService
    {
        IEnumerable<Model.Domain.Event> GetAllEvent(string userId);
        Model.Domain.Event GetEvent(int id);
        bool ValidateEventInfo(Model.Domain.Event eventInfo);
    }
}
