using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class GraphUser
    {
        public string id { get; set; }
        public string mail { get; set; }
        public string displayName { get; set; }
        public string userPrincipalName { get; set; }
    }
}
