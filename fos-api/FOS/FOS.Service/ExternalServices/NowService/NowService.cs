using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Dto;
using FOS.Model.Mapping;
using FOS.Services.ExternalServices.NowService.Convert;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FOS.Services.ExternalServices.NowService
{
    public class NowService : IExternalService
    {
        APIs _apis;
        NowServiceConfiguration apisJson;
        public NowService(APIs apis)
        {
            _apis = apis;
            apisJson = JsonConvert.DeserializeObject<NowServiceConfiguration>(_apis.JSONData);
        }

        public List<FoodCatalogue> GetFoodCatalogues(DeliveryInfos delivery)
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
                return ConvertJson.ConvertString2ListFoodCatalogue(result);
            }
            else
            {
                return null;
            }
        }
        public string GetDeliveryFromUrl(Province province, DeliveryInfos delivery)
        {
            //Get function
            APIDetail api = apisJson.GetDeliveryFromUrl;
            //Set Fields
            api.AvailableParams.Where(a => a.FieldName == "url").FirstOrDefault().ValueDefault
                = province.name_url + "/" + delivery.url_rewrite_name;
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = method.CallApi();
            if (response.Result.IsSuccessStatusCode)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                return result;
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

        public List<FoodCatalogue> GetFoodFromDelivery(List<DeliveryInfos> deliveries)
        {
            throw new NotImplementedException();
        }
    }
}
