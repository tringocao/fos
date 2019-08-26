﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FOS.API
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(object jsonContent, HttpRequestMessage request)
        {
            JsonContent = jsonContent;
            Request = request;
        }

        public Object JsonContent { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            response.Content = new ObjectContent(JsonContent.GetType(), JsonContent, new JsonMediaTypeFormatter());
            return response;
        }
    }
}