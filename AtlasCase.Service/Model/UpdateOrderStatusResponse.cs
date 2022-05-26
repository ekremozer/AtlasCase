using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasCase.Service.Model
{
    public class UpdateOrderStatusResponse
    {
        public string CustomerOrderNo { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string OrderStatus { get; set; }
        public DateTime? OrderStatusUpdateAt { get; set; }
    }
}
