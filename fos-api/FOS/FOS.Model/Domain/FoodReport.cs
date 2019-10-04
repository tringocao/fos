using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class FoodReport
    {
        public string FoodId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Picture { get; set; }
        public List<Comment> Comments { get; set; }
        public string TotalComment { get; set; }
        public int Amount { get; set; }
        public int Total { get; set; }
        public int NumberOfUser { get; set; }
        public List<string> UserIds { get; set; }
    }
}
