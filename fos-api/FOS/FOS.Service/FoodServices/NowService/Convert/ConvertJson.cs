using FOS.Model.Dto;
using FOS.Model.Mapping;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.FoodServices.NowService.Convert
{
    public static class ConvertJson
    {
        public static List<Restaurant> ConvertString2ListRestaurant(string result)
        {
            //TODO
            //throw new NotImplementedException(result);

            dynamic data = JObject.Parse(result);
            List<Restaurant> newList = new List<Restaurant>();
            foreach (var id in data.reply.search_result[0].restaurant_ids)//get the fisrt catalogue
            {
                Restaurant item = new Restaurant();
                item.restaurant_id = id;
                newList.Add(item);
            }
            return newList;
        }
        public static List<Food> ConvertString2ListFood(string result)
        {
            //TODO
            throw new NotImplementedException(result);

            dynamic data = JObject.Parse(result);
            List<Food> newList = new List<Food>();
            foreach (var dish in data.reply.menu_infos[0].dishes)
            {
                Food item = new Food();
                item.id = dish.id;
                newList.Add(item);
            }
            return newList;
        }
        public static List<DeliveryInfos> ConvertString2ListDeliveryInfos(string result)
        {
            dynamic data = JObject.Parse(result);
            List<DeliveryInfos> newList = new List<DeliveryInfos>();
            JsonDtoMapper<DeliveryInfos> map = new JsonDtoMapper<DeliveryInfos>();

            foreach (var delivery in data.reply.delivery_infos)
            {
                newList.Add(map.ToDto(delivery));

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
    }
}
