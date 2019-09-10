using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class Operating
    {
        [JsonProperty("close_time")]
        public string CloseTime { get; set; }
        [JsonProperty("open_time")]
        public string OpenTime { get; set; }
    }
}
