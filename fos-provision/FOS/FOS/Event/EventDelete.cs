using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Event
{
    public static class EventDelete
    {
        public static void Delete(ClientContext clientContext)
        {
            DeleteList(clientContext);
            DeleteContentType(clientContext);
            DeleteSiteColumn(clientContext);
        }
        public static void DeleteList(ClientContext clientContext)
        {
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event List", "list") == 1)
            {
                Web web = clientContext.Web;

                List list = web.Lists.GetByTitle("Event List");

                list.DeleteObject();

                clientContext.ExecuteQuery();

                Console.WriteLine("Delete Event List");
            }
            else
            {
                Console.WriteLine("Event List does not exist");
            }
        }
        public static void DeleteContentType(ClientContext clientContext)
        {
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Content Type", "contenttypeName") == 1)
            {
                ContentType ct = clientContext.Web.ContentTypes.GetById("0x0101009189AB5D3D2647B580F011DA2F356FB7");

                ct.DeleteObject();
                clientContext.ExecuteQuery();
                Console.WriteLine("Delete Content Type");
            }
            else
            {
                Console.WriteLine("Content Type does not exist");
            }
        }
        public static void DeleteSiteColumn(ClientContext clientContext)
        {
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Id", "field") == 1)
            {
                Field EventHostColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Id");
                EventHostColumn.DeleteObject();
                clientContext.ExecuteQuery();

            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Title", "field") == 1)
            {
                Field EventHostColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Title");
                EventHostColumn.DeleteObject();
                clientContext.ExecuteQuery();

            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Host", "field") == 1)
            {
                Field EventHostColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Host");
                EventHostColumn.DeleteObject();
                clientContext.ExecuteQuery();

            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Restaurant", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Restaurant");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();

            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Maximum budget", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Maximum budget");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();

            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Time to close", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Time to close");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();

            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Time to reminder", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Time to reminder");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Participants", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Participants");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Category", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Category");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event RestaurantId", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event RestaurantId");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event ServiceId", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event ServiceId");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event DeliveryId", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event DeliveryId");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event CreatedUserId", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event CreatedUserId");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event HostId", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event HostId");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Type", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Type");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Date", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Date");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Status", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event Status");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Event  ParticipantsJson", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Event  ParticipantsJson");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();
            }
            Console.WriteLine("Delete Event columns!");
            Console.WriteLine("Provision finished!");
        }
    }
}
