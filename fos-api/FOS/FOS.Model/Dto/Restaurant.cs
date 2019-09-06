using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    class Restaurant
    {
        public int Id { get; set; }
        public bool Stared { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Address { get; set; }
        public string Promotion { get; set; }
        public string Open { get; set; }
        public string Delivery_id { get; set; }
        public string UrlRewriteName { get; set; }
        public string Picture { get; set; }

    }
    class RestaurantDetail
    {
        public int Rating { get; set; }
        public int TotalReview { get; set; }

    }
}
