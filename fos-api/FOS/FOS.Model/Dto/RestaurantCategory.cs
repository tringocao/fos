using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class RestaurantCategory
    {
        public string code { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string parent_category_id { get; set; }

        ///public List<RestaurantCategory> categories { get; set; }

    }
}
