using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Core.EntityFramework;

namespace AtlasCase.Data.Entities
{
    public class Material : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Order> Orders { get; set; }

        public Material()
        {
            Orders = new HashSet<Order>();
        }
    }
}
