using FOS.Common;
using FOS.Model.Domain;
using FOS.Services;
using FOS.Services.EventServices;
using FOS.Services.Providers;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;

namespace FOS.API.Controllers
{
    public class SPListController : ApiController
    {
        IOAuthService _oAuthService;
        IGraphApiProvider _graphApiProvider;
        ISharepointContextProvider _sharepointContextProvider;
        IEventService _eventService;

        public SPListController(IOAuthService oAuthService, IGraphApiProvider graphApiProvider, ISharepointContextProvider sharepointContextProvider, IEventService eventService)
        {
            _oAuthService = oAuthService;
            _graphApiProvider = graphApiProvider;
            _sharepointContextProvider = sharepointContextProvider;
            _eventService = eventService;
        }
        // GET api/splist/getlist/{list-id}
        public async Task<HttpResponseMessage> GetList(string Id)
        {
            return await _graphApiProvider.SendAsync(HttpMethod.Get, "sites/lists/" + Id, null);
        }
        // POST api/splist/addlistitem/{list-id}/
        public async Task<HttpResponseMessage> AddListItem(string Id, [FromBody]JSONRequest item)
        {
            return await _graphApiProvider.SendAsync(HttpMethod.Post, "sites/lists/" + Id + "/items/", item.data);
        }
        public string GetAllOrder()
        {
            using (ClientContext clientContext = _sharepointContextProvider.GetSharepointContextFromUrl(APIResource.SHAREPOINT_CONTEXT + "/sites/FOS/"))
            {
                var web = clientContext.Web;
                var list = web.Lists.GetByTitle("Event List");
                clientContext.Load(list);
                clientContext.ExecuteQuery();

                var collListItem = list.GetItems(CamlQuery.CreateAllItemsQuery());
                clientContext.Load(collListItem);
                clientContext.ExecuteQuery();

                return JsonConvert.SerializeObject(_eventService.MapSharepointEventListToEventModel(collListItem));
            }
        }
    }
}
