using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class Group
    {
        public string Id { get; set; }
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public List<ScoredEmailAddresses> ScoredEmailAddresses { get; set; }
    }
}
