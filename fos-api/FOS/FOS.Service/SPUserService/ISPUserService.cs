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
        Task<List<User>> GetUsers();
        Task<Model.Dto.GraphUser> GetCurrentUserGraph();
        Task<User> GetCurrentUser();
        Task<User> GetUserById(string Id);
        Task<List<User>> GetGroups();
        Task<byte[]> GetAvatar(string Id, string avatarName);
        Task<List<User>> GetUsersByName(string searchName);

        Task<List<User>> GroupListMemers(string groupId);
        Task<List<User>> SearchGroupOrUserByName(string searchName);
        Task<bool> ValidateIsHost(int eventId);
    }
}
