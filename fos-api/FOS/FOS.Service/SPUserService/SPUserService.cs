using FOS.Common;
using FOS.Services.Providers;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.SPUserService
{
    public class SPUserService : ISPUserService
    {
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;

        public SPUserService(IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider)
        {
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
        }

        public async Task<List<Model.Dto.User>> GetUsers()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "users", null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject(resultGroup);

                List<Model.Dto.User> jsonUsers = response.value.ToObject<List<Model.Dto.User>>();

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
        public async Task<Model.Dto.User> GetCurrentUser()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "me", null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                Model.Dto.User response = JsonConvert.DeserializeObject<Model.Dto.User>(resultGroup);

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
                dynamic response = JsonConvert.DeserializeObject(resultGroup);


                Model.Domain.User jsonUsers = response.value.ToObject<Model.Dto.User>();

                return jsonUsers;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<Model.Dto.User>> GetGroups()
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "groups", null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject(resultGroup);

                List<Model.Dto.User> jsonGroup = response.value.ToObject<List<Model.Dto.User>>();

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

        public async Task<List<Model.Dto.User>> GetUsersByName(string searchName)
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "me/people/?$search=" + searchName, null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject(resultGroup);

                List<Model.Dto.User> jsonUsers = response.value.ToObject<List<Model.Dto.User>>();

                return jsonUsers;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }

        public async Task<List<Model.Dto.User>> GroupListMemers(string groupId)
        {
            var result = await _graphApiProvider.SendAsync(HttpMethod.Get, "groups/" + groupId + "/members", null);
            if (result.IsSuccessStatusCode)
            {
                var resultGroup = await result.Content.ReadAsStringAsync();
                dynamic response = JsonConvert.DeserializeObject(resultGroup);

                List<Model.Dto.User> jsonUsers = response.value.ToObject<List<Model.Dto.User>>();

                return jsonUsers;
            }
            else
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }
        }
    }
}
