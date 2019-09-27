using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class UserOrder
    {
        public User User { get; set; }
        public string Food { get; set; }
        public int Price { get; set; }
        public int PayExtra { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
