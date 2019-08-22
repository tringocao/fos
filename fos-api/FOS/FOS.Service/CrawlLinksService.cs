
using FOS.Model.Domain;
using FOS.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services
{
    public class CrawlLinksService : ICrawlLinksService
    {
        private IFOSCrawlLinksRepository _crawlLinksRepo;

        public CrawlLinksService(IFOSCrawlLinksRepository crawlLinksRepo)
        {
            this._crawlLinksRepo = crawlLinksRepo;
        }

        public IEnumerable<APIs> GetAllFOSCrawlLinks()
        {
            return _crawlLinksRepo.GetAllFOSCrawlLinks();
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

        public async Task<string> GetByIdAsync(int businessId)
        {
            APIs api = _crawlLinksRepo.GetFOSCrawlLinksById(businessId);
            HttpClient h = new HttpClient();
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

            //h.DefaultRequestHeaders.Add("x-foody-api-version", "1");
            //h.DefaultRequestHeaders.Add("x-foody-access-token", "");
            //h.DefaultRequestHeaders.Add("x-foody-app-type", "1004");
            //h.DefaultRequestHeaders.Add("x-foody-client-id", "");
            //h.DefaultRequestHeaders.Add("x-foody-client-language", "vi");
            //// h.DefaultRequestHeaders.Add("Content-Type","application/json; charset=UTF-8");
            //h.DefaultRequestHeaders.Add("x-foody-client-type", "1");
            //h.DefaultRequestHeaders.Add("x-foody-client-version", "3.0.0");

            //string myJSONRequest = "{\"restaurant_ids\":[71299]}";
            HttpContent requestContent = new StreamContent(GenerateStreamFromString(myJSONRequest.ToString()));
            var response = await h.PostAsync(api.Link, requestContent);
            //response.Wait(3000);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var s = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                return s.ToString();
            }
            else
            {
                return "Fail";
            }
        }
        public APIs GetById(int Id)
        {
            return _crawlLinksRepo.GetFOSCrawlLinksById(Id);

        }
    }
}
