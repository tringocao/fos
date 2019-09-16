using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Order
{
    class OrderContentType
    {
        const string contentTypeId = "0x0101009189AB5D3D2647B580F011DA2F356FB1";
        const string contentTypeName = "Order Content Type";

        public static void Create(ClientContext context)
        {

            if (Helper.CheckHelper.isExist_Helper(context, "Order Content Type", "contenttypeName") == 0)
            {
                ContentTypeCollection contentTypeColl = context.Web.ContentTypes;

                ContentTypeCreationInformation contentTypeCreation = new ContentTypeCreationInformation();
                contentTypeCreation.Name = contentTypeName;
                contentTypeCreation.Description = contentTypeName;
                contentTypeCreation.Group = "Order Content Type Group";
                contentTypeCreation.Id = contentTypeId;
                ContentType ct = contentTypeColl.Add(contentTypeCreation);
                context.Load(ct);
                context.ExecuteQuery();
                Console.WriteLine("create content type successfully");
                AddSiteColumn(context);
            }
            else
            {
                Console.WriteLine("content already exist!");
            }
        }
        public static void AddSiteColumn(ClientContext clientContext)
        {
            Web rootWeb = clientContext.Site.RootWeb;
            Field OrderEventId = rootWeb.Fields.GetByInternalNameOrTitle("OrderEventId");
            Field OrderUser = rootWeb.Fields.GetByInternalNameOrTitle("OrderUser");
            Field OrderUserDelegate = rootWeb.Fields.GetByInternalNameOrTitle("OrderUserDelegate");
            Field OrderInfo = rootWeb.Fields.GetByInternalNameOrTitle("OrderInfo");

            ContentType sessionContentType = rootWeb.ContentTypes.GetById(contentTypeId);

            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = OrderEventId
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = OrderUser
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = OrderUserDelegate
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = OrderInfo
            });
            sessionContentType.Update(true);
            clientContext.ExecuteQuery();

            Console.WriteLine("Add site columns to content type");
        }
    }
}
