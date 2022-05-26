using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Core.EntityFramework;
using AtlasCase.Data.Entities;

namespace AtlasCase.Data.Core
{
    public class BaseDataCreator
    {
        public static void Create(EfContext context)
        {
            AddOrderStatuses(context);
            AddUnits(context);
            AddWeightUnits(context);
            AddMaterial(context);
            AddTempOrder(context);
        }
        private static void AddOrderStatuses(EfContext context)
        {
            context.OrderStatuses.Add(new OrderStatus { Name = "Sipariş Alındı" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Yola Çıktı" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Dağıtım Merkezinde" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Teslim Edildi" });
            context.OrderStatuses.Add(new OrderStatus { Name = "Teslim Edilemedi" });
            context.SaveChanges();
        }
        private static void AddUnits(EfContext context)
        {
            context.Units.Add(new Unit { Name = "Adet" });
            context.Units.Add(new Unit { Name = "Koli" });
            context.Units.Add(new Unit { Name = "Paket" });
            context.Units.Add(new Unit { Name = "Palet" });
            context.SaveChanges();
        }

        private static void AddWeightUnits(EfContext context)
        {
            context.WeightUnits.Add(new WeightUnit { Name = "Kilo" });
            context.WeightUnits.Add(new WeightUnit { Name = "Ton" });
            context.SaveChanges();
        }

        private static void AddMaterial(EfContext context)
        {
            context.Materials.Add(new Material { Name = "Çelik", Code = "MLZ-1234" });
            context.SaveChanges();
        }

        public static void AddTempOrder(EfContext context)
        {
            var orderNo = DateTime.Now.ToString("yyMMddHHmm");
            orderNo += 1.ToString("D4");
            var order = new Order
            {
                OrderNo = orderNo,
                CustomerOrderNo = "FRM-00001",
                PortAddress = "Ay Mah. Çam Sok. İstanbul",
                ArrivalAddress = "Mit Mah. Kam Sok. İstanbul",
                Quantity = 12,
                UnitId = 3,
                Weight = 1,
                WeightUnitId = 2,
                MaterialId = 1,
                Note = "10'a kadar teslim",
                OrderStatusId = 1,
            };
            context.Orders.Add(order);
            context.SaveChanges();
        }
    }
}