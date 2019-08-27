using FOS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FOS.API
{
    public interface IGraphHttpClient
    {
        HttpRequestMessage GetRequestMessage(string path, HttpMethod method);
    }
    public class GraphHttpClient : IGraphHttpClient
    {
        IOAuthService _oAuthService;

        public GraphHttpClient(IOAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }
        public HttpRequestMessage GetRequestMessage(string path, HttpMethod method)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, path);

            var accessToken = _oAuthService.GetTokenFromCookie()._accessToken;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return request;
        }
    }
}
