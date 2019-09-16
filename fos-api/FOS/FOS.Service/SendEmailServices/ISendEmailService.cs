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
        Task SendEmailAsync(string idEvent, string html);
        Task SendEmailToNotOrderedUserAsync(IEnumerable<UserNotOrderMailInfo> users, string emailTemplateJson);
        string Parse<T>(string text, T modelparse);
    }
}
