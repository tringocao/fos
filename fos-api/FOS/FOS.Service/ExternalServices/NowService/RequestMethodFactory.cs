using FOS.Model.Domain;
using FOS.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.ExternalServices.NowService
{
    public class RequestMethodFactory
    {
        HttpClient h;
        APIDetail api;
        public RequestMethodFactory(APIDetail api)
        {
            this.api = api;
            this.h = new HttpClient();
        }
        private void SetHeader()
        {
            if (api.AvailableHeaders.Count() > 0)
            {
                foreach (var header in api.AvailableHeaders)
                {
                    h.DefaultRequestHeaders.Add(header.FieldName, header.ValueDefault);
                }
            }
        }
        private StringBuilder SetBody()
        {
            StringBuilder myJSONRequest = new StringBuilder();
            if (api.AvailableBodys.Count() > 0)
            {
                myJSONRequest.Append("{");

                foreach (var body in api.AvailableBodys)
                {
                    myJSONRequest.Append(",\"" + body.FieldName + "\":" + body.ValueDefault);
                }
                myJSONRequest.Remove(1, 1);// remove first comma
                myJSONRequest.Append("}");
                return myJSONRequest;
            }
            else return myJSONRequest;
        }
        private StringBuilder SetParams()
        {
            StringBuilder myJSONRequest = new StringBuilder();

            if (api.AvailableParams.Count() > 0)
            {
                myJSONRequest.Append("?");
                foreach (var body in api.AvailableParams)
                {
                    myJSONRequest.Append("&" + body.FieldName + "=" + body.ValueDefault);
                }
                myJSONRequest.Remove(1, 1);// remove first &
                return myJSONRequest;
            }
            else return myJSONRequest;
        }
        private async Task<HttpResponseMessage> PostMethodAsync(StringBuilder myJSONRequest)
        {
            HttpContent requestContent = new StreamContent(Config.GenerateStreamFromString(myJSONRequest.ToString()));
            var response = await h.PostAsync(api.API, requestContent);
            return response;
        }
        private async Task<HttpResponseMessage> GetMethod(StringBuilder myJSONRequest)
        {
            var response = await h.GetAsync(api.API + myJSONRequest.ToString());
            return response;
        }
        public async Task<HttpResponseMessage> CallApiAsync()
        {
            SetHeader();
            switch (api.RequestMethod)
            {
                case RequestMethod.Post:
                    {
                        return await PostMethodAsync(SetBody());
                    }
                case RequestMethod.Get:
                    {
                        return await GetMethod(SetParams());
                    }
                default:
                    throw new Exception();
            }
        }

    }
}
