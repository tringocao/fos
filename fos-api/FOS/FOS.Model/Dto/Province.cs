using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class Province
    {
        public string CountryId { get; set; }
        public string Id { get; set; }
        //public int province_id { get; set; }
        public string Name { get; set; }
        public string NameUrl { get; set; }
        public string Service { get; set; }
        public string District { get; set; }
    }
}
