using FOS.Model.Domain.NowModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class EventPromotion
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public List<Model.Domain.NowModel.Promotion> Promotions { get; set; }
    }
}
