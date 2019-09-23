using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class UserNotOrderEmail
    {
        public string UserEmail { get; set; }
        public string OrderId { get; set; }
    }
}
