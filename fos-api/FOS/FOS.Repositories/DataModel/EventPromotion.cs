using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Repositories.DataModel
{
    public class EventPromotion
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Promotions { get; set; }
    }
}
