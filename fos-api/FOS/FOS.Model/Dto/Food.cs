using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class Food
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Photos { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public bool IsChecked { get; set; }

    }
}
