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
        Task<Model.Dto.User> GetCurrentUser();
        Task<Model.Domain.User> GetUserById(string Id);
        Task<string> GetGroups();
        Task<byte[]> GetAvatarByUserId(string Id);
    }
}
