using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtlasCase.Service.Model
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public string PortAddress { get; set; }
        public string ArrivalAddress { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string Unit { get; set; }
        public int Weight { get; set; }
        public int WeightUnitId { get; set; }
        public string WeightUnit { get; set; }
        public int MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string Note { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
    }
}
