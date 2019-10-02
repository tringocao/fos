using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.DataModel
{
    public class GraphUserInGroup
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string UserPrincipalName { get; set; }
    }
}
