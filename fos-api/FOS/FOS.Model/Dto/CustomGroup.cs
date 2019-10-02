using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class CustomGroup
    {
        public Guid ID { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<GraphUser> Users { get; set; }
    }
}
