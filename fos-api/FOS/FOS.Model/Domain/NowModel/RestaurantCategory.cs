using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class RestaurantCategory
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("parent_category_id")]
        public string ParentCategoryId { get; set; }
        [JsonProperty("categories")]
        public List<RestaurantCategory> Categories { get; set; }

    }
}
