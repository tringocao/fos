using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.SPUserService
{
    public interface ISPUserService
    {
        Task<List<Model.Dto.User>> GetUsers();
        Task<Model.Dto.User> GetCurrentUser();
        Task<User> GetUserById(string Id);
        Task<List<Model.Dto.User>> GetGroups();
        Task<byte[]> GetAvatarByUserId(string Id);
        Task<List<Model.Dto.User>> GetUsersByName(string searchName);
    }
}
