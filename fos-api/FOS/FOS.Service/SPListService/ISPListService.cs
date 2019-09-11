using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.SPListService
{
    public interface ISPListService
    {
        Task AddListItem(string Id, JsonRequest item);
        string AddEventListItem(string Id, EventListItem item);
        Task UpdateListItem(string Id, EventListItem item);
    }
}
