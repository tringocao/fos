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
        Task<Model.Domain.User> GetUserByIdsDomain(string Id);
        Task<List<Model.Dto.User>> GetUsers();
        Task<Model.Dto.GraphUser> GetCurrentUserGraph();
        Task<Model.Dto.User> GetCurrentUser();
        Task<Model.Dto.User> GetUserById(string Id);
        Task<List<Model.Dto.User>> GetGroups();
        Task<byte[]> GetAvatar(string Id, string avatarName);
        Task<List<Model.Dto.User>> GetUsersByName(string searchName);

        Task<List<Model.Dto.User>> GroupListMemers(string groupId);
    }
}
