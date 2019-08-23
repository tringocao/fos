using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Services.FoodServices.NowService.Convert;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FOS.Services.FoodServices.NowService
{
    public class NowService : IFoodService
    {
        APIs _apis;
        NowServiceConfiguration apisJson;
        public NowService(APIs apis)
        {
            _apis = apis;
            apisJson = JsonConvert.DeserializeObject<NowServiceConfiguration>(_apis.JSONData);
        }

        public List<Food> GetFoods(DeliveryInfos delivery)
        {
            //Get function
            APIDetail api = apisJson.GetDeliveryDishes;
            //Set Fields
            api.AvailableParams.Where(a => a.FieldName == "request_id").FirstOrDefault().ValueDefault
                = delivery.delivery_id.ToString();
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = method.CallApi();
            if (response.Result.IsSuccessStatusCode)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                return ConvertJson.ConvertString2ListFood(result);
            }
            else
            {
                return null;
            }
        }

        public List<Restaurant> GetRestaurants(Province province)
        {
            //Get function
            APIDetail api = apisJson.SearchRestaurantsInProvince;
            //Set Fields
            api.AvailableBodys.Where(a => a.FieldName == "city_id").FirstOrDefault().ValueDefault
                = province.id.ToString();// 217 is id of HCM city
            api.AvailableBodys.Where(a => a.FieldName == "keyword").FirstOrDefault().ValueDefault
                = "\"\"";
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = method.CallApi();
            if (response.Result.IsSuccessStatusCode)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                return ConvertJson.ConvertString2ListRestaurant(result);
            }
            else
            {
                return null;
            }
        }
        public List<DeliveryInfos> GetRestaurantDeliveryInfor(Restaurant restaurant)
        {
            //Get function
            APIDetail api = apisJson.GetRestaurantDeliveryInfor;
            //Set Fields
            api.AvailableBodys.Where(a => a.FieldName == "restaurant_ids").FirstOrDefault().ValueDefault
                = "[" + restaurant.restaurant_id.ToString() + "]";// 217 is id of HCM city
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = method.CallApi();
            if (response.Result.IsSuccessStatusCode)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                return ConvertJson.ConvertString2ListDeliveryInfos(result);
            }
            else
            {
                return null;
            }
        }
        public List<Province> GetMetadata()
        {
            //Get function
            APIDetail api = apisJson.GetMetadata;
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = method.CallApi();
            if (response.Result.IsSuccessStatusCode)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                return ConvertJson.ConvertString2ListProvinces(result);
            }
            else
            {
                return null;
            }
        }
        public string GetNameService()
        {
            return "NowService";
        }

        public List<DeliveryInfos> GetRestaurantsDeliveryInfor(List<Restaurant> restaurant)
        {
            //Get function
            APIDetail api = apisJson.GetRestaurantDeliveryInfor;
            //Set Fields
            StringBuilder rid = new StringBuilder();
            foreach (var r in restaurant)
            {
                rid.Append("," + r.restaurant_id);
            }
            rid.Remove(0, 1);// remove the first comma
            api.AvailableBodys.Where(a => a.FieldName == "restaurant_ids").FirstOrDefault().ValueDefault
                = "[" + rid + "]";// 217 is id of HCM city
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = method.CallApi();
            if (response.Result.IsSuccessStatusCode)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                return ConvertJson.ConvertString2ListDeliveryInfos(result);
            }
            else
            {
                return null;
            }
        }
    }
}
