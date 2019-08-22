using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.RequestMethods
{
    public class PostMethod: IRequestMethod
    {
        APIs api;
        HttpClient h;
        public PostMethod()
        {        }
        public void setAPI(APIs api)
        {
            this.api = api;
        }
        static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public async Task<string> GetResultAsync()
        {
            h = new HttpClient();
            foreach (var header in api.header)
            {
                h.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            StringBuilder myJSONRequest = new StringBuilder();
            myJSONRequest.Append("{");
            foreach (var body in api.body)
            {
                myJSONRequest.Append(",\"" + body.Key + "\":" + body.Value);
            }
            myJSONRequest.Remove(1, 1);
            myJSONRequest.Append("}");
            HttpContent requestContent = new StreamContent(GenerateStreamFromString(myJSONRequest.ToString()));
            var response = await h.PostAsync(api.Link, requestContent);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                //var s = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                return result;
            }
            else
            {
                return "Fail";
            }
        }
    }
}
