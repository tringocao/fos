using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class Province
    {
        public string country_id { get; set; }
        public string id { get; set; }
        //public int province_id { get; set; }

        public string name { get; set; }
        public string name_url { get; set; }
        public dynamic service { get; set; }
        public dynamic district { get; set; }

    }
}
