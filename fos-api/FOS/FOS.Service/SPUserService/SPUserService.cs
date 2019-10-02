using FOS.Common;
using FOS.Model.Mapping;
using FOS.Services.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;
using FOS.Services.EventServices;

namespace FOS.Services.SPUserService
{
    public class SPUserService : ISPUserService
    {
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;
        IEventService _eventService;
        public SPUserService(IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider, IUserDtoMapper userDtoMapper, IEventService eventService)
        {
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
            _eventService = eventService;
        }

        public async Task<List<Model.Domain.User>> GetUsers()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "users", null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject(resultGroup);

                List<Model.Domain.User> jsonUsers = response.value.ToObject<List<Model.Domain.User>>();

                return jsonUsers;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<Model.Dto.GraphUser> GetCurrentUserGraph()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "me", null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                Model.Dto.GraphUser response = JsonConvert.DeserializeObject<Model.Dto.GraphUser>(resultGroup);

                return response;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }
        public async Task<Model.Domain.User> GetCurrentUser()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "me", null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                Model.Domain.User response = JsonConvert.DeserializeObject<Model.Domain.User>(resultGroup);

                return response;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }
        public async Task<Model.Domain.User> GetUserById(string Id)
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "users/" + Id, null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Model.Domain.User>(resultGroup);

                return response;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<Model.Domain.User>> GetGroups()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "groups", null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject(resultGroup);

                List<Model.Domain.User> jsonGroup = response.value.ToObject<List<Model.Domain.User>>();

                return jsonGroup;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<byte[]> GetAvatar(string Id, string avatarName)
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "users/" + Id + "/photos/48x48/$value", null);

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsByteArrayAsync();
            }
            else
            {
                String[] spearator = { " " };

                String[] strlist = avatarName.Split(spearator, StringSplitOptions.RemoveEmptyEntries);
                string avatarUrl = "";
                if (strlist.Length == 1)
                {
                    string firstName = strlist[0];
                    avatarUrl = "https://ui-avatars.com/api/?name=" + firstName;
                }
                else
                {
                    string firstName = strlist[0];
                    string lastName = strlist[1];

                    string fullNameUrl = firstName + "+" + lastName;
                    avatarUrl = "https://ui-avatars.com/api/?name=" + fullNameUrl;
                }

                var http = new HttpClient();
                byte[] response = { };
                await Task.Run(async () => response = await http.GetByteArrayAsync(avatarUrl));

                return response;
            }
        }

        public async Task<List<Model.Domain.User>> GetUsersByName(string searchName)
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "me/people/?$search=" + searchName, null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject(resultGroup);

                List<Model.Domain.User> jsonUsers = response.value.ToObject<List<Model.Domain.User>>();

                return jsonUsers;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<Model.Domain.User>> GroupListMemers(string groupId)
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "groups/" + groupId + "/members", null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject(resultGroup);

                List<Model.Domain.User> jsonUsers = response.value.ToObject<List<Model.Domain.User>>();

                return jsonUsers;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }
        public async Task<List<User>> SearchGroupOrUserByName(string searchName)
        {
            List<Model.Domain.User> listSearchUser = new List<User>();
            String queryString = String.Format("users?$filter=startswith(displayName,'{0}')", searchName);
            var resultSearchUser = await _graphApiProvider.SendAsync(HttpMethod.Get, queryString, null);

            if (resultSearchUser.IsSuccessStatusCode)
            {
                var resultUser = await resultSearchUser.Content.ReadAsStringAsync();
                dynamic responseUser = JsonConvert.DeserializeObject(resultUser);

                var newList = responseUser.value.ToObject<List<Model.Domain.User>>();
                if (newList.Count > 0)
                {
                    listSearchUser.AddRange(newList);

                }
            }
            else
            {
                throw new Exception(await resultSearchUser.Content.ReadAsStringAsync());
            }

            List<Model.Domain.User> listSearchGroup = new List<User>();
            String queryStringGroup = String.Format("groups?$filter=startswith(displayName,'{0}')", searchName);
            var resultSearchGroup = await _graphApiProvider.SendAsync(HttpMethod.Get, queryStringGroup, null);

            if (resultSearchGroup.IsSuccessStatusCode)
            {
                var resultGroup = await resultSearchGroup.Content.ReadAsStringAsync();
                dynamic responseUser = JsonConvert.DeserializeObject(resultGroup);

                List<Model.Domain.User> newList = responseUser.value.ToObject<List<Model.Domain.User>>();
                if (newList.Count > 0)
                {
                    listSearchUser.AddRange(newList);

                }
            }
            else
            {
                throw new Exception(await resultSearchUser.Content.ReadAsStringAsync());
            }

            return listSearchUser;
        }
        public async Task<bool> ValidateIsHost(int eventId)
        {
            try
            {
                //get host's event
                Event eventInfo =  _eventService.GetEvent(eventId);
                var hostId = eventInfo.HostId;
                //get current user
                User currentUser = await GetCurrentUser();

                if(hostId != currentUser.Id)
                {
                    return false;
                }

                return true;
            }catch(Exception e)
            {
                throw e;
            }
        }
    }
}
