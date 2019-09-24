using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class UpdateEvent
    {
        public List<GraphUser> RemoveListUser { get; set; }
        public List<GraphUser> NewListUser { get; set; }
        public string IdEvent { get; set; }
    }
}
