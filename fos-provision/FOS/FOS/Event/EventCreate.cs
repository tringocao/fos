using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Event
{
    public static class EventCreate
    {
        public static void Create(ClientContext context)
        {
            EventSiteColumns.CreateColumn(context);
            EventContentType.Create(context);
            EventList.CreateList(context);
            Console.WriteLine("Provision finished!");
        }
    }
}
