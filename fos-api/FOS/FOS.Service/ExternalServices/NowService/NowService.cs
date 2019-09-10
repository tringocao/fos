using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FOS.Model.Domain;
using FOS.Model.Domain.NowModel;
using FOS.Model.Mapping;
using FOS.Services.ExternalServices.NowService.Convert;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FOS.Services.ExternalServices.NowService
{
    public class NowService : IExternalService
    {
        Apis _apis;
        NowServiceConfiguration apisJson;
        public NowService(Apis apis)
        {
            _apis = apis;
            apisJson = JsonConvert.DeserializeObject<NowServiceConfiguration>(_apis.JSONData);
        }
        public async Task<DeliveryDetail> GetRestaurantDetailAsync(Restaurant restaurant)
        {
            //Get function
            APIDetail api = apisJson.GetRestaurantDetail;
            //Set Fields
            api.AvailableParams.Where(a => a.FieldName == "request_id").FirstOrDefault().ValueDefault
                = restaurant.DeliveryId.ToString();
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = await method.CallApiAsync();
            var result = response.Content.ReadAsStringAsync().Result;
            return ConvertJson.ConvertString2DeliveryInfos(result);

        }
        public async Task<List<FoodCategory>> GetFoodCataloguesAsync(DeliveryInfos delivery)
        {
            //Get function
            APIDetail api = apisJson.GetDeliveryDishes;
            //Set Fields
            api.AvailableParams.Where(a => a.FieldName == "request_id").FirstOrDefault().ValueDefault
                = delivery.DeliveryId.ToString();
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = await method.CallApiAsync();
            var result = response.Content.ReadAsStringAsync().Result;
            return ConvertJson.ConvertString2ListFoodCatalogue(result);

        }
        public async Task<string> GetDeliveryFromUrlAsync(Province province, DeliveryInfos delivery)
        {
            //Get function
            APIDetail api = apisJson.GetDeliveryFromUrl;
            //Set Fields
            api.AvailableParams.Where(a => a.FieldName == "url").FirstOrDefault().ValueDefault
                = province.NameUrl + "/" + delivery.UrlRewriteName;
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = await method.CallApiAsync();
            return response.Content.ReadAsStringAsync().Result;
        }
        public async Task<List<Restaurant>> GetRestaurantsAsync(Province province, string keyword, List<RestaurantCategory> category)
      {
            //Get function
            APIDetail api = apisJson.SearchRestaurantsInProvince;
            //Set Fields
            api.AvailableBodys.Where(a => a.FieldName == "city_id").FirstOrDefault().ValueDefault
                = province.Id.ToString();// 217 is id of HCM city
            api.AvailableBodys.Where(a => a.FieldName == "keyword").FirstOrDefault().ValueDefault
                = "" + keyword + "";
            if (category !=null )
            {
                StringBuilder icate = new StringBuilder();
                foreach (var c in category)
                {
                    icate.Append(",{\"code\":" + c.Code + ",\"id\":" + c.Id + "}");
                }
                if (category.Count() != 0) icate.Remove(0, 1);// remove the first comma
                api.AvailableBodys.Where(a => a.FieldName == "combine_categories").FirstOrDefault().ValueDefault
                    = "[" + icate + "]";
            }

            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = await method.CallApiAsync();
            var result = response.Content.ReadAsStringAsync().Result;
            return ConvertJson.ConvertString2ListRestaurant(result);
        }
        public async Task<List<DeliveryInfos>> GetRestaurantDeliveryInforAsync(Restaurant restaurant)
        {
            //Get function
            APIDetail api = apisJson.GetRestaurantDeliveryInfor;
            //Set Fields
            api.AvailableBodys.Where(a => a.FieldName == "restaurant_ids").FirstOrDefault().ValueDefault
                = "[" + restaurant.RestaurantId.ToString() + "]";// 217 is id of HCM city
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = await method.CallApiAsync();
            var result = response.Content.ReadAsStringAsync().Result;
            return ConvertJson.ConvertString2ListDeliveryInfos(result);

        }
        public async Task<List<Province>> GetMetadataForProvinceAsync()
        {
            //Get function
            APIDetail api = apisJson.GetMetadataForProvince;
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = await method.CallApiAsync();
            var result = response.Content.ReadAsStringAsync().Result;
            return ConvertJson.ConvertString2ListProvinces(result);
        }
        public async Task<List<RestaurantCategory>> GetMetadataForCategoryAsync()
        {
            //Get function
            APIDetail api = apisJson.GetMetadataForCategory;
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = await method.CallApiAsync();
            var result = response.Content.ReadAsStringAsync().Result;
            return ConvertJson.ConvertString2ListRestaurantCategories(result);
          
        }
        public string GetNameService()
        {
            return "NowService";
        }

        public async Task<List<DeliveryInfos>> GetRestaurantsDeliveryInforAsync(List<Restaurant> restaurant)
        {
            //Get function
            APIDetail api = apisJson.GetRestaurantDeliveryInfor;
            //Set Fields
            StringBuilder rid = new StringBuilder();
            foreach (var r in restaurant)
            {
                rid.Append("," + r.RestaurantId);
            }
            if (restaurant.Count() != 0) rid.Remove(0, 1);// remove the first comma
            api.AvailableBodys.Where(a => a.FieldName == "restaurant_ids").FirstOrDefault().ValueDefault
                = "[" + rid + "]";// 217 is id of HCM city
            //Call API
            RequestMethodFactory method = new RequestMethodFactory(api);
            var response = await method.CallApiAsync();
            var result = response.Content.ReadAsStringAsync().Result;
            
            return ConvertJson.ConvertString2ListDeliveryInfos(result);

        }
        Task<List<FoodCategory>> GetFoodFromDelivery(List<DeliveryInfos> deliveries)
        {
            throw new NotImplementedException();
        }

        Task<List<FoodCategory>> IExternalService.GetFoodFromDelivery(List<DeliveryInfos> deliveries)
        {
            throw new NotImplementedException();
        }
    }
}
