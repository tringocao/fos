using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class CategoryGroup
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
        public List<Category> Categories { get; set; }
    }
}
