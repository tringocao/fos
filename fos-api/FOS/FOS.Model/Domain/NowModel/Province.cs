using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class Province
    {
        [JsonProperty("country_id")]
        public string CountryId { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        //public int province_id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("name_url")]
        public string NameUrl { get; set; }


    }
}
