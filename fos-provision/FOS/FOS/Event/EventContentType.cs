using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS
{
    public static class EventContentType
    {
        const string contentTypeId = "0x0101009189AB5D3D2647B580F011DA2F356FB7";
        const string contentTypeName = "Event Content Type";

        public static void Create(ClientContext context)
        {

            if (Helper.CheckHelper.isExist_Helper(context, contentTypeName, "contenttypeName") == 0)
            {
                ContentTypeCollection contentTypeColl = context.Web.ContentTypes;

                ContentTypeCreationInformation contentTypeCreation = new ContentTypeCreationInformation();
                contentTypeCreation.Name = contentTypeName;
                contentTypeCreation.Description = contentTypeName;
                contentTypeCreation.Group = "Event Content Type Group";
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
            Field EventId = rootWeb.Fields.GetByInternalNameOrTitle("EventId");
            Field EventTitle = rootWeb.Fields.GetByInternalNameOrTitle("EventTitle");
            Field EventHost = rootWeb.Fields.GetByInternalNameOrTitle("EventHost");
            Field EventRestaurant = rootWeb.Fields.GetByInternalNameOrTitle("EventRestaurant");
            Field EventMaximumBudget = rootWeb.Fields.GetByInternalNameOrTitle("EventMaximumBudget");
            Field EventTimeToClose = rootWeb.Fields.GetByInternalNameOrTitle("EventTimeToClose");
            Field EventTimeToReminder = rootWeb.Fields.GetByInternalNameOrTitle("EventTimeToReminder");
            Field EventParticipants = rootWeb.Fields.GetByInternalNameOrTitle("EventParticipants");
            Field EventCategory = rootWeb.Fields.GetByInternalNameOrTitle("EventCategory");
            Field EventRestaurantId = rootWeb.Fields.GetByInternalNameOrTitle("EventRestaurantId");
            Field EventServiceId = rootWeb.Fields.GetByInternalNameOrTitle("EventServiceId");
            Field EventDeliveryId = rootWeb.Fields.GetByInternalNameOrTitle("EventDeliveryId");
            Field EventCreatedUserId = rootWeb.Fields.GetByInternalNameOrTitle("EventCreatedUserId");
            Field EventHostId = rootWeb.Fields.GetByInternalNameOrTitle("EventHostId");
            Field EventType = rootWeb.Fields.GetByInternalNameOrTitle("EventTypes");
            Field EventDate = rootWeb.Fields.GetByInternalNameOrTitle("EventDate");
            Field EventStatus = rootWeb.Fields.GetByInternalNameOrTitle("EventStatus");
            Field EventParticipantsJson = rootWeb.Fields.GetByInternalNameOrTitle("EventParticipantsJson");
            Field EventIsReminder = rootWeb.Fields.GetByInternalNameOrTitle("EventIsReminder");
            ContentType sessionContentType = rootWeb.ContentTypes.GetById(contentTypeId);

            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventId
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventTitle
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventHost
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventRestaurant
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventMaximumBudget
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventTimeToClose
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventTimeToReminder
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventParticipants
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventCategory
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventRestaurantId

            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventServiceId
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventDeliveryId
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventCreatedUserId
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventHostId
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventType
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventDate
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventStatus
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventParticipantsJson
            });
            sessionContentType.FieldLinks.Add(new FieldLinkCreationInformation
            {
                Field = EventIsReminder
            });
            sessionContentType.Update(true);
            clientContext.ExecuteQuery();

            Console.WriteLine("Add site columns to content type");
        }
    }
}
