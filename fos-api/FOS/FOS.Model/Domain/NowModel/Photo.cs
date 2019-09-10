using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class Photo
    {
        [JsonProperty("value")]
        public string Value { get; set; }

    }
}
