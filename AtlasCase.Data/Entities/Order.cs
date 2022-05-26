using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Core.EntityFramework;

namespace AtlasCase.Data.Entities
{
    public class Order : BaseEntity<int>
    {
        public string OrderNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public string PortAddress { get; set; }
        public string ArrivalAddress { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public int Weight { get; set; }
        public int WeightUnitId { get; set; }
        public WeightUnit WeightUnit { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public string Note { get; set; }
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime? OrderStatusUpdateAt { get; set; }

        public Order()
        {
            OrderStatusId = 1;
        }
    }
}
