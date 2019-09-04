using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.SPUserService
{
    public interface ISPUserService
    {
        Task<string> GetUsers();
        Task<Model.Domain.User> GetCurrentUser();
        Task<Model.Domain.User> GetUserById(string Id);
        Task<string> GetAvatarById(string Id, string size);
    }
}
