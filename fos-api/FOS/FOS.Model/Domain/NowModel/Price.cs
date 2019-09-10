using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class Price
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
        [JsonProperty("value")]
        public float Value { get; set; }

    }
}
