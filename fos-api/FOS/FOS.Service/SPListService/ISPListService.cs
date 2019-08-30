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
        Task AddListItem(string Id, JSONRequest item);
    }
}
