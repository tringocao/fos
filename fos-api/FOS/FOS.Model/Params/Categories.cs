using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Params
{
    public class Categories
    {
        [JsonProperty("code")]
        public string code { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("parent_category_id")]
        public string parent_category_id { get; set; }
        [JsonProperty("categories")]
        public List<Categories> categories { get; set; }
    }
}
