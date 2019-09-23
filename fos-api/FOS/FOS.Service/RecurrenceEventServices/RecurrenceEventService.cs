using FOS.Model.Domain;
using FOS.Repositories.Mapping;
using FOS.Repositories.Repositories;
using FOS.Services.SendEmailServices;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace FOS.Services.RecurrenceEventServices
{
    public class RecurrenceEventService : IRecurrenceEventService
    {
        IRecurrenceEventRepository _eventRepository;
        IRecurrenceEventMapper _recurrenceEventMapper;
        ISendEmailService _sendEmailService;
        Func<IRecurrenceEventRepository> _func;
        public RecurrenceEventService(IRecurrenceEventRepository eventRepository, IRecurrenceEventMapper recurrenceEventMapper, ISendEmailService sendEmailService, Func<IRecurrenceEventRepository> func )
        {
            _eventRepository = eventRepository;
            _recurrenceEventMapper = recurrenceEventMapper;
            _sendEmailService = sendEmailService;
            _func = func;
        }

        public bool AddRecurrenceEvent(RecurrenceEvent recurrenceEvent)
        {
            Repositories.DataModel.RecurrenceEvent temp = new Repositories.DataModel.RecurrenceEvent();
            _recurrenceEventMapper.MapToEfObject(temp, recurrenceEvent);
            return _eventRepository.AddRecurrenceEvent(temp);
        }

        public IEnumerable<RecurrenceEvent> GetAllRecurrenceEvents()
        {
            return _eventRepository.GetAllRecurrenceEvents().Select(r => _recurrenceEventMapper.MapToDomain(r));
        }

        public RecurrenceEvent GetById(int id)
        {
            return _recurrenceEventMapper.MapToDomain(_eventRepository.GetById(id));
        }
        public RecurrenceEvent GetByUserId(string userId)
        {
            return _recurrenceEventMapper.MapToDomain(_eventRepository.GetByUserId(userId));
        }
        public bool DeleteById(int id)
        {
            return _eventRepository.DeleteRecurrenceEvent(id);
        }
        public bool UpdateRecurrenceEvent(RecurrenceEvent recurrenceEvent)
        {

            Repositories.DataModel.RecurrenceEvent temp = new Repositories.DataModel.RecurrenceEvent();
            _recurrenceEventMapper.MapToEfObject(temp, recurrenceEvent);
            return _eventRepository.UpdateRecurrenceEvent(temp);
        }
        private bool CheckTypeRepeat(Model.Domain.RecurrenceEvent item)
        {
            DateTime now = DateTime.Now.ToLocalTime();
            switch (item.TypeRepeat)
            {
                case RepeateType.Daily:
                    {
                        //+-1mins: if have a missed duration unneed -> will remove in the next session, 
                        //because the time of next session is 1 hour, which is different from StartTempDate 
                        //(StartTempDate saves next day)
                        if (now.AddMinutes(-1) <= item.StartTempDate && now.AddHours(1).AddMinutes(1) > item.StartTempDate)
                        {
                            item.StartTempDate = item.StartTempDate.AddDays(1);
                            return true;
                        }
                        break;
                    }
                case RepeateType.EveryWorkDay:
                    {
                        if (now.AddMinutes(-1) <= item.StartTempDate && now.AddHours(1).AddMinutes(1) > item.StartTempDate)
                        {
                            item.StartTempDate = item.StartTempDate.AddDays(1);
                            return true;
                        }
                        break;
                    }
                case RepeateType.Monthly:
                    {
                        if (now.AddMinutes(-1) <= item.StartTempDate && now.AddHours(1).AddMinutes(1) > item.StartTempDate)
                        {
                            item.StartTempDate = item.StartTempDate.AddMonths(1);
                            return true;
                        }
                        break;
                    }
                case RepeateType.Weekly:
                    {
                        if (now.AddMinutes(-1) <= item.StartTempDate && now.AddHours(1).AddMinutes(1) > item.StartTempDate)
                        {
                            item.StartTempDate = item.StartTempDate.AddDays(7);
                            return true;
                        }
                        break;
                    }
                default:
                    {
                        if (now.AddMinutes(-1) <= item.StartTempDate && now.AddHours(1).AddMinutes(1) > item.StartTempDate)
                        {
                            return true;
                        }
                        break;
                    }

            }
            return false;
        }
        public void RunThisTask(RecurrenceEvent item)
        {
            DateTime now = DateTime.Now.ToLocalTime();
            if (now.AddMinutes(-1) <= item.StartTempDate && now.AddHours(1).AddMinutes(1) < item.EndDate)
            {
                if (CheckTypeRepeat(item))
                {
                    //if (isWindowService) item.Version = Int32.Parse(ConfigurationSettings.AppSettings["versionFirstReminder"]);
                    Task.Factory.StartNew(() => RunReminderAsync(item));
                }
            }
        }
        public void checkRemindedTask()
        {
            try
            {
                WriteToFile("Service is started at " + DateTime.Now.ToLocalTime());

                var list = GetAllRecurrenceEvents().ToList();
                DateTime now = DateTime.Now.ToLocalTime();
                foreach (var item in list)
                {
                    if (now.AddMinutes(-1) <= item.StartTempDate && now.AddHours(1).AddMinutes(1) < item.EndDate)
                    {
                        if (CheckTypeRepeat(item))
                        {
                            WriteToFile("Service is started at " + DateTime.Now.ToLocalTime());
                            //if (isWindowService) item.Version = Int32.Parse(ConfigurationSettings.AppSettings["versionFirstReminder"]);
                            Task.Factory.StartNew(() =>RunReminderAsync(item));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToFile("Service is stopped at " + ex.ToString());

            }
        }
        public void RunReminderAsync(Model.Domain.RecurrenceEvent item)
        {
            try
            {
                UpdateRecurrenceEvent(item);
                if (item.TypeRepeat == RepeateType.EveryWorkDay)
                {
                    if (item.StartTempDate.DayOfWeek == DayOfWeek.Saturday
                        || item.StartTempDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        return;
                    }
                }
                int versionForThisTask = item.Version;
                DateTime startTaskTime = DateTime.Now.ToLocalTime();
                TimeSpan startTime = item.StartDate.TimeOfDay;
                TimeSpan delayStartTime = item.StartDate.AddMinutes(1).TimeOfDay;
                while (true)
                {
                    if (startTaskTime <= DateTime.Now.ToLocalTime() && startTaskTime.AddHours(1) > DateTime.Now.ToLocalTime())// if 60:01 > start, runed 60 time before
                    {
                        if (startTime <= DateTime.Now.ToLocalTime().TimeOfDay
                            && DateTime.Now.ToLocalTime().TimeOfDay < delayStartTime)
                        {
                            item = _recurrenceEventMapper.MapToDomain(_func.Invoke().GetById(item.Id));

                            if (item.Version != versionForThisTask)// false when update or sent email before
                            {
                                break;
                            }
                            using (var clientContext = GetClientContext())
                            {
                                var emailTemplateDictionary = GetEmailTemplate(@"App_Data\RemindEmailTemplate.json");
                                emailTemplateDictionary.TryGetValue("Body", out string body);
                                emailTemplateDictionary.TryGetValue("Subject", out string subject);
                                SendEmail(clientContext, "admin@devpreciovn.onmicrosoft.com", item.UserMail, _sendEmailService.Parse(body, item), subject);
                                WriteToFile("Service is sent email at! " + item.Id);
                            }
                            break;

                        }
                        Task.Delay(60000).Wait();
                    }
                }
                WriteToFile("Service is ended " + item.Id);
            }
            catch (Exception e)
            {
                WriteToFile("Service is stopped at " + e.ToString() + "Id: " + item.Id);
            }
        }
        public ClientContext GetClientContext()
        {
            var siteUrl = "https://devpreciovn.sharepoint.com/sites/FOS/";
            var loginName = ConfigurationSettings.AppSettings["loginName"];
            var passWord = ConfigurationSettings.AppSettings["passWord"];
            var securePassword = new SecureString();
            passWord.ToCharArray().ToList().ForEach(c => securePassword.AppendChar(c));

            using (var clientContext = new ClientContext(siteUrl))
            {
                clientContext.Credentials = new SharePointOnlineCredentials(loginName, securePassword);
                return clientContext;
            }
        }
        public Dictionary<string, string> GetEmailTemplate(string templateLink)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + templateLink;
            string emailTemplateJson = System.IO.File.ReadAllText(path);

            var emailTemplateDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(emailTemplateJson);

            return emailTemplateDictionary;
        }
        public void SendEmail(ClientContext clientContext, string fromMail, string toMail, string body, string subject)
        {
            var emailp = new EmailProperties();

            emailp.To = new List<string>() { toMail };
            emailp.From = fromMail;
            emailp.Body = body;
            emailp.Subject = subject;

            Utility.SendEmail(clientContext, emailp);
            clientContext.ExecuteQuery();
        }
        public void WriteToFile(string Message)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
                if (!System.IO.File.Exists(filepath))
                {
                    // Create a file to write to.   
                    using (StreamWriter sw = System.IO.File.CreateText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
                else
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
