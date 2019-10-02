using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Dto;

namespace FOS.Services.SendEmailServices
{
    public interface ISendEmailService
    {
        string Parse<T>(string text, T modelparse);
        Task SendEmailAsync(string idEvent, string html);
        Task SendEmailToReOrderEventAsync(List<Model.Domain.UserReorder> users, string emailTemplateJson);
        Task SendEmailToNotOrderedUserAsync(IEnumerable<UserNotOrderMailInfo> users, string emailTemplateJson);
        Task SendEmailToAlreadyOrderedUserAsync(List<UserFeedbackMailInfo> users, string emailTemplateJson);
        Task SendMailUpdateEvent(List<Model.Domain.GraphUser> removeListUser, List<Model.Domain.User> newListUser, string idEvent, string html);
        Task<IEnumerable<UserNotOrderMailInfo>> FilterUserIsParticipant(IEnumerable<UserNotOrderMailInfo> users);
        Task SendCancelEventMail(List<Model.Domain.EventUsers> listUser, Dictionary<string, string> emailTemplateDictionary);
        Dictionary<string, string> GetEmailTemplate(string templateLink);
        Task<List<Model.Domain.EventUsers>> FilterUser(List<Model.Domain.EventUsers> users);
    }
}
