using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class DiscountEvent
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Dictionary<string, float> Discounts { get; set; }
    }
}
