using FOS.Model.Domain.NowModel;
using FOS.Model.Mapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.ExternalServices.NowService.Convert
{
    public static class ConvertJson
    {
        public static DeliveryDetail ConvertString2DeliveryInfos(string result)
        {
            dynamic data = JObject.Parse(result);
            JsonDtoMapper<DeliveryDetail> map = new JsonDtoMapper<DeliveryDetail>();
            DeliveryDetail deliveryInfos = new DeliveryDetail();
            if (data.result == "success")
            {
                deliveryInfos = JsonConvert.DeserializeObject<DeliveryDetail>(data.reply.delivery_detail.ToString());
            }
            return deliveryInfos;
        }
        public static List<Restaurant> ConvertString2ListRestaurant(string result)
        {
            //TODO
            //throw new NotImplementedException(result);

            dynamic data = JObject.Parse(result);
            List<Restaurant> newList = new List<Restaurant>();
            if (data.result == "success")
            {
                if (data.reply.search_result.Count < 1) return newList;
                foreach (var id in data.reply.search_result[0].restaurant_ids)//get the fisrt catalogue
                {
                    Restaurant item = new Restaurant();
                    item.RestaurantId = id;
                    newList.Add(item);
                }
            }
            return newList;
        }
        public static List<FoodCategory> ConvertString2ListFoodCatalogue(string result)
        {
            dynamic data = JObject.Parse(result);
            List<FoodCategory> newList = new List<FoodCategory>();
            JsonDtoMapper<FoodCategory> map = new JsonDtoMapper<FoodCategory>();

            foreach (var dish in data.reply.menu_infos)
            {
                newList.Add(JsonConvert.DeserializeObject<FoodCategory>(dish.ToString()));
            }
            return newList;
        }
        public static List<DeliveryInfos> ConvertString2ListDeliveryInfos(string result)
        {
            dynamic data = JObject.Parse(result);
            List<DeliveryInfos> newList = new List<DeliveryInfos>();
            JsonDtoMapper<DeliveryInfos> map = new JsonDtoMapper<DeliveryInfos>();
            if (data.result == "success")
            {
                foreach (var delivery in data.reply.delivery_infos)
                {
                    newList.Add(JsonConvert.DeserializeObject<DeliveryInfos>(delivery.ToString()));

                }
            }
            return newList;
        }
        public static List<Province> ConvertString2ListProvinces(string result)
        {
            dynamic data = JObject.Parse(result);
            List<Province> newList = new List<Province>();
            JsonDtoMapper<Province> map = new JsonDtoMapper<Province>();
            foreach (var province in data.reply.metadata.province)
            {
                newList.Add(JsonConvert.DeserializeObject<Province>(province.ToString()));
            }
            return newList;
        }
        public static List<RestaurantCategory> ConvertString2ListRestaurantCategories(string result)
        {
            dynamic data = JObject.Parse(result);
            List<RestaurantCategory> newList = new List<RestaurantCategory>();
            JsonDtoMapper<RestaurantCategory> map = new JsonDtoMapper<RestaurantCategory>();
            foreach (var categories in data.reply.country.now_services[0].categories)
            {
                newList.Add(JsonConvert.DeserializeObject<RestaurantCategory>(categories.ToString()));
            }
            return newList;
        }
    }
}
