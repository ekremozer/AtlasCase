using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Core.EntityFramework;

namespace AtlasCase.Data.Entities
{
    public class WeightUnit : BaseEntity<int>
    {
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }

        public WeightUnit()
        {
            Orders = new HashSet<Order>();
        }        
    }
}
