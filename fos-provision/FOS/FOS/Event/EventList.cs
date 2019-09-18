using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Event
{
    public static class EventList
    {
        const string contentTypeId = "0x0101009189AB5D3D2647B580F011DA2F356FB7";
        const string ListName = "Event List";

        public static void CreateList(ClientContext context)
        {
            if (Helper.CheckHelper.isExist_Helper(context, ListName, "list") == 0)
            {
                ListCreationInformation creationInfo = new ListCreationInformation();
                creationInfo.Title = ListName;
                creationInfo.Description = ListName;
                creationInfo.TemplateType = (int)ListTemplateType.GenericList;

                List createList;

                createList = context.Web.Lists.Add(creationInfo);
                context.Load(createList);
                context.ExecuteQuery();
                Console.WriteLine("Create list!");
                AddContentTypeToList(createList, context);
                CreateEventListView(createList, context);
            }
            else
            {
                Console.WriteLine("List already exist!");
            }
        }

        public static void AddContentTypeToList(List targetList, ClientContext context)
        {
            targetList.ContentTypesEnabled = true;
            targetList.Update();
            context.ExecuteQuery();

            var contentType = context.Site.RootWeb.ContentTypes.GetById(contentTypeId);
            targetList.ContentTypes.AddExistingContentType(contentType);
            context.ExecuteQuery();

            Console.WriteLine("Add content type to list!");
        }
        public static void CreateEventListView(List targetList, ClientContext context)
        {
            ViewCollection viewColl = targetList.Views;

            string[] viewFields = { "EventId", "EventTitle", "EventHost", "EventRestaurant", "EventMaximumBudget", "EventTimeToClose", "EventTimeToReminder", "EventParticipants", "EventCategory", "EventRestaurantId", "EventServiceId", "EventDeliveryId", "EventCreatedUserId", "EventHostId", "EventTypes", "EventDate", "EventStatus", "EventParticipantsJson", "EventIsReminder" };

            ViewCreationInformation creationInfo = new ViewCreationInformation();
            creationInfo.Title = "Event View";
            creationInfo.RowLimit = 50;
            creationInfo.ViewFields = viewFields;
            creationInfo.ViewTypeKind = ViewType.None;
            creationInfo.SetAsDefaultView = true;
            viewColl.Add(creationInfo);
            context.ExecuteQuery();
            Console.WriteLine("created view");
        }
    }
}
