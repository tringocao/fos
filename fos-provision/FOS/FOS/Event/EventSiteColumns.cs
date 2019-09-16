using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS
{
    public static class EventSiteColumns
    {
        public static void CreateColumn(ClientContext clientContext)
        {
            try
            {
                Web rootWeb = clientContext.Site.RootWeb;

                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Id", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Id' Name='EventId' Group='Event Column' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }

                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Title", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Title' Name='EventTitle' Group='Event Column' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }

                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Host", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Host' Name='EventHost' Group='Event Column' Type='User' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Restaurant", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Restaurant' Name='EventRestaurant' Group='Event Column' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Maximum budget", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Maximum budget' Name='EventMaximumBudget' Group='Event Column' Type='Number'>" +
                        "<Default>0</Default></Field>", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Time to close", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Time to close' Name='EventTimeToClose' Group='Event Column' Type='DateTime'>" +
                        "<Default>[Today]</Default></Field>", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Time to reminder", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Time to reminder' Name='EventTimeToReminder' Group='Event Column' Type='DateTime'>" +
                        "<Default>[Today]</Default></Field>", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Participants", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Participants' Name='EventParticipants' Group='Event Column' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }

                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Category", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Category' Name='EventCategory' Group='Event Column' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event RestaurantId", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event RestaurantId' Name='EventRestaurantId' Group='Event Column' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event ServiceId", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event ServiceId' Name='EventServiceId' Group='Event Column' Type='Number' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event DeliveryId", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event DeliveryId' Name='EventDeliveryId' Group='Event Column' Type='Number' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event CreatedUserId", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event CreatedUserId' Name='EventCreatedUserId' Group='Event Column' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event HostId", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event HostId' Name='EventHostId' Group='Event Column' Type='Text' />", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }
                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Types", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Type' Name='EventTypes' Group='Event Column' Type='Choice' Format='Dropdown'>"
                       +"<Default>Open</Default>"
                       + "<CHOICES>"
                       + "    <CHOICE>Open</CHOICE>"
                       + "    <CHOICE>Close</CHOICE>"
                       + "</CHOICES>"
                       + "</Field>", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }

                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Date", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Date' Name='EventDate' Group='Event Column' Type='DateTime'>" +
                        "<Default>[Today]</Default></Field>", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }

                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event Status", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event Status' Name='EventStatus' Group='Event Column' Type='Choice' Format='Dropdown'>"
                       + "<Default>Opened</Default>"
                       + "<CHOICES>"
                       + "    <CHOICE>Opened</CHOICE>"
                       + "    <CHOICE>Closed</CHOICE>"
                       + "    <CHOICE>Error</CHOICE>"
                       + "</CHOICES>"
                       + "</Field>", false, AddFieldOptions.AddFieldInternalNameHint);
                    clientContext.ExecuteQuery();
                }

                if (Helper.CheckHelper.isExist_Helper(clientContext, "Event  ParticipantsJson", "field") == 0)
                {
                    rootWeb.Fields.AddFieldAsXml("<Field DisplayName='Event ParticipantsJson' Name='EventParticipantsJson' Group='Event Column' Type='Note' NumLines='1' RichText='FALSE' Sortable='FALSE'>" +
                        "</Field>", false, AddFieldOptions.AddFieldInternalNameHint);
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
