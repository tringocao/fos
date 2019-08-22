using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FOS.Services.FoodServices.NowService
{
    public class NowService : IFoodService
    {
        APIsDTO _apis;
        NowServiceConfiguration apisJson;
        public NowService(APIsDTO apis)
        {
            _apis = apis;
            apisJson = JsonConvert.DeserializeObject<NowServiceConfiguration>(_apis.JSONData);
        }
        public List<Food> GetFoods(Restaurant restaurant)
        {
            var RestaurantId = restaurant.delivery_id;
            HttpClient h = new HttpClient();
            APIDetail api = apisJson.GetAllRestaurant;
            foreach (var header in api.AvailableHeaders)
            {
                h.DefaultRequestHeaders.Add(header.FieldName, header.ValueDefault);
            }
            StringBuilder myJSONRequest = new StringBuilder();
            myJSONRequest.Append("{");

            foreach (var body in api.AvailableBodys)
            {
                if (body.FieldName == "restaurant_ids") body.ValueDefault = "[" + RestaurantId + "]";
                myJSONRequest.Append(",\"" + body.FieldName + "\":" + body.ValueDefault);
            }
            myJSONRequest.Remove(1, 1);
            myJSONRequest.Append("}");

            HttpContent requestContent = new StreamContent(GenerateStreamFromString(myJSONRequest.ToString()));
            var response =  h.PostAsync(api.API, requestContent);
            response.Wait(3000);
            if (response.Result.IsSuccessStatusCode)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                //var s = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                return ConvertString2ListObject<Food>(result);
            }
            else
            {
                return null;
            }
        }

        public  List<Restaurant> GetRestaurantsAsync()
        {
            //NowServiceConfiguration a = new NowServiceConfiguration();
            //string json = JsonConvert.SerializeObject(a, Formatting.Indented);
            HttpClient h = new HttpClient();
            APIDetail api = apisJson.Search_Global;
            foreach (var header in api.AvailableHeaders)
            {
                h.DefaultRequestHeaders.Add(header.FieldName, header.ValueDefault);
            }
            StringBuilder myJSONRequest = new StringBuilder();
            myJSONRequest.Append("{");

            foreach (var body in api.AvailableBodys)
            {
                myJSONRequest.Append(",\"" + body.FieldName + "\":" + body.ValueDefault);
            }
            myJSONRequest.Remove(1, 1);
            myJSONRequest.Append("}");

            HttpContent requestContent = new StreamContent(GenerateStreamFromString(myJSONRequest.ToString()));
            var response = h.PostAsync(api.API, requestContent);
            response.Wait(3000);
            if (response.Result.IsSuccessStatusCode)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                //var s = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                return ConvertString2ListObject<Restaurant>(result);
            }
            else
            {
                return null;
            }
        }
        private List<T> ConvertString2ListObject<T>(string result)
        {
            dynamic data = JObject.Parse(result);
            
            return data.reply.search_result[0].restaurant_ids;
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
        public string GetNameService()
        {
            return "NowService";
        }
    }
}
