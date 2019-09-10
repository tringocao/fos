using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class PromotionGroup
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
