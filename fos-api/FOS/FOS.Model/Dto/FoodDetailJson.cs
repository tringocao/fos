using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class FoodDetailJson
    {
        public string IdFood { get; set; }
        public Dictionary<string, string> Value { get; set; }
    }
}
