using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class APIs
    {
        public int ID { get; set; }
        public string Link { get; set; }
        public int HostID { get; set; }
        public string Description { get; set; }
        public string Request_Method { get; set; }
        public Dictionary<string,string> header { get; set; }
        public Dictionary<string, string> body { get; set; }

    }
}
