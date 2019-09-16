using System;
using Microsoft.SharePoint.Client;
using System.Configuration;
using System.Security;
using System.Linq;


namespace FOS
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var siteUrl = "https://devpreciovn.sharepoint.com/sites/FOS/";

                var loginName = ConfigurationSettings.AppSettings["loginName"];
                var passWord = ConfigurationSettings.AppSettings["passWord"];

                var securePassword = new SecureString();
                passWord.ToCharArray().ToList().ForEach(c => securePassword.AppendChar(c));

                using (var context = new ClientContext(siteUrl))
                {
                    context.Credentials = new SharePointOnlineCredentials(loginName, securePassword);

                    String ChooseInput = "";

                    int ChooseAction = 3;
                    do
                    {
                        Console.Write("Action: \t 1.Add Event List \t 2.Delete Event List" +
                            "\n  \t\t 3.Add Order List \t 4.Delete Order List \t 0.Exit: ");
                        ChooseInput = Console.ReadLine();

                        ChooseAction = Convert.ToInt32(ChooseInput);

                        if (ChooseAction == 1)
                        {
                            Console.WriteLine("Waiting...");
                            Event.EventCreate.Create(context);
                        }
                        else if (ChooseAction == 2)
                        {
                            Console.WriteLine("Waiting...");
                            Event.EventDelete.Delete(context);
                        }
                        else if (ChooseAction == 3)
                        {
                            Console.WriteLine("Waiting...");
                            Order.OrderCreate.Create(context);
                        }
                        else if (ChooseAction == 4)
                        {
                            Console.WriteLine("Waiting...");
                            Order.OrderDelete.Delete(context);
                        }
                        else if (ChooseAction == 0)
                        {
                            return;
                        }
                    } while (true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            Console.ReadKey();
        }
    }
}
