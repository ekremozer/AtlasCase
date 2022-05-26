using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Core.EntityFramework;

namespace AtlasCase.Data.Entities
{
    public class OrderStatus : BaseEntity<int>
    {
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }

        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }
    }
}
