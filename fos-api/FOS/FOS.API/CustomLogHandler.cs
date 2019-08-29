using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FOS.API
{
    public class CustomLogHandler : DelegatingHandler
    {
        private const string Origin = "Origin";

        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var isCorsRequest = request.Headers.Contains(Origin);
            var isPreflightRequest = request.Method == HttpMethod.Options;

            var logMetadata = BuildRequestMetadata(request);

            var response = await base.SendAsync(request, cancellationToken);
            //response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

            logMetadata = BuildResponseMetadata(logMetadata, response);

            await SendToLog(logMetadata);   

            return response;
        }

        private LogMetadata BuildRequestMetadata(HttpRequestMessage request)
        {
            LogMetadata log = new LogMetadata
            {
                RequestMethod = request.Method.Method,
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
            return log;
        }

        private LogMetadata BuildResponseMetadata(LogMetadata logMetadata, HttpResponseMessage response)
        {
            logMetadata.ResponseStatusCode = response.StatusCode;
            logMetadata.ResponseTimestamp = DateTime.Now;
            logMetadata.ResponseContentType = response.Content.Headers.ContentType.MediaType;
            return logMetadata;
        }

        private async Task<bool> SendToLog(LogMetadata logMetadata)
        {
            // TODO: Write code here to store the logMetadata instance to a pre-configured log store...
            return true;
        }
    }
}