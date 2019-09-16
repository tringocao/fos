using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Order
{
    public static class OrderDelete
    {
        public static void Delete(ClientContext clientContext)
        {
            DeleteList(clientContext);
            DeleteContentType(clientContext);
            DeleteSiteColumn(clientContext);
        }
        public static void DeleteList(ClientContext clientContext)
        {
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Order List", "list") == 1)
            {
                Web web = clientContext.Web;

                List list = web.Lists.GetByTitle("Order List");

                list.DeleteObject();

                clientContext.ExecuteQuery();

                Console.WriteLine("Delete Order List");
            }
            else
            {
                Console.WriteLine("Order List does not exist");
            }
        }
        public static void DeleteContentType(ClientContext clientContext)
        {
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Order Content Type", "contenttypeName") == 1)
            {
                ContentType ct = clientContext.Web.ContentTypes.GetById("0x0101009189AB5D3D2647B580F011DA2F356FB1");

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
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Order Event Id", "field") == 1)
            {
                Field OrderHostColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Order Event Id");
                OrderHostColumn.DeleteObject();
                clientContext.ExecuteQuery();

            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Order User", "field") == 1)
            {
                Field OrderHostColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Order User");
                OrderHostColumn.DeleteObject();
                clientContext.ExecuteQuery();

            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Order User Delegate", "field") == 1)
            {
                Field OrderHostColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Order User Delegate");
                OrderHostColumn.DeleteObject();
                clientContext.ExecuteQuery();

            }
            if (Helper.CheckHelper.isExist_Helper(clientContext, "Order Info", "field") == 1)
            {
                Field deleteColumn = clientContext.Web.Fields.GetByInternalNameOrTitle("Order Info");
                deleteColumn.DeleteObject();
                clientContext.ExecuteQuery();

            }
            Console.WriteLine("Delete Order columns!");
            Console.WriteLine("Provision finished!");
        }
    }
}
