using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class ScoredEmailAddresses
    {
        public string Address { get; set; }
        public string RelevanceScore { get; set; }
        public string SelectionLikelihood { get; set; }
    }
}
