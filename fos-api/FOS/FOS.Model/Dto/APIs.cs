using FOS.Model.Domain;
using FOS.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class APIs
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ServiceKind TypeService { get; set; }
        public string JSONData { get; set; }

    }
}
