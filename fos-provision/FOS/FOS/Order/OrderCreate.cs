using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Order
{
    public static class OrderCreate
    {
        public static void Create(ClientContext context)
        {
            Order.OrderSiteColumns.CreateColumn(context);
            Order.OrderContentType.Create(context);
            Order.OrderList.CreateList(context);
            Console.WriteLine("Provision finished!");
        }
    }
}
