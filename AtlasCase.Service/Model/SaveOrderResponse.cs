using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasCase.Service.Model
{
    public class SaveOrderResponse
    {
        public string CustomerOrderNo { get; set; }
        public string OrderNo { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
