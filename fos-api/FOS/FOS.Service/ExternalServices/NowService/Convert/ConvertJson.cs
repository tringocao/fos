using FOS.Model.Dto;
using FOS.Model.Mapping;
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
                    item.restaurant_id = id;
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
                newList.Add(map.ToDto(dish));
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
                    newList.Add(map.ToDto(delivery));

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
                newList.Add(map.ToDto(province));
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
                newList.Add(map.ToDto(categories));
            }
            return newList;
        }
    }
}
