using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class EventPromotion
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public List<Model.Dto.Promotion> Promotions { get; set; }
    }
}
