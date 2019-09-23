using Microsoft.SharePoint.Client;
using System.Collections.Generic;

namespace FOS.Services.FosCoreService
{
    public interface IFosCoreService
    {
        string BuildLink(string link, string text);
        ListItemCollection GetListEventOpened(ClientContext clientContext);
        void ChangeStatusToClose(ClientContext clientContext, ListItem element);
        Dictionary<string, string> GetEmailTemplate(string templateLink);
        void SendEmail(ClientContext clientContext, string fromMail, string toMail, string body, string subject);
        ClientContext GetClientContext();
        string Parse<T>(string text, T modelparse);
        List<Model.Domain.UserNotOrderEmail> GetUserNotOrderEmail(string idEvent);
        void SendMailRemider(IEnumerable<Model.Dto.UserNotOrderMailInfo> lstUser);
    }
}