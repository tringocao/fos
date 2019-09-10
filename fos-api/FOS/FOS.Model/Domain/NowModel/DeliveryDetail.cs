using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class DeliveryDetail
    {
        [JsonProperty("rating")]

        public dynamic Rating { get; set; }

    }
}
