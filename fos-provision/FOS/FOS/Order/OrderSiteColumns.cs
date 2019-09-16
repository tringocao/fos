using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Order
{
    public static class OrderSiteColumns
    {
        public static void CreateColumn(ClientContext clientContext)
        {
            try
            {
                Web rootWeb = clientContext.Site.RootWeb;

                if (Helper.CheckHelper.isExist_Helper(clientContext, "Order Event Id", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Order Event Id' Name='OrderEventId' Group='Order Column' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }

                if (Helper.CheckHelper.isExist_Helper(clientContext, "Order User", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Order User' Name='OrderUser' Group='Order Column' Type='User' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }

                if (Helper.CheckHelper.isExist_Helper(clientContext, "Order User Delegate", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Order User Delegate' Name='OrderUserDelegate' Group='Order Column' Type='User' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Order Info", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Order Info' Name='OrderInfo' Group='Order Column' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }

                Console.WriteLine("Create site column successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
