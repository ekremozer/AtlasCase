using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AtlasCase.Data.Core.EntityFramework
{
    public class ModelBuilderHelper
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(x => x.Id, "order_Id").IsUnique();
                entity.HasIndex(a => a.OrderNo, "order_No").IsUnique();
                entity.HasIndex(a => a.CustomerOrderNo, "customer_Order_No").IsUnique();

                entity.HasOne(x => x.OrderStatus).WithMany(x => x.Orders).HasForeignKey(x => x.OrderStatusId);
                entity.HasOne(x => x.Unit).WithMany(x => x.Orders).HasForeignKey(x => x.UnitId);
                entity.HasOne(x => x.WeightUnit).WithMany(x => x.Orders).HasForeignKey(x => x.WeightUnitId);
                entity.HasOne(x => x.Material).WithMany(x => x.Orders).HasForeignKey(x => x.MaterialId);

                entity.Property(a => a.PortAddress).HasMaxLength(500);
                entity.Property(a => a.ArrivalAddress).HasMaxLength(500);

            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasIndex(x => x.Id, "order_Status_Id").IsUnique();
                entity.Property(a => a.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasIndex(x => x.Id, "unit_Status_Id").IsUnique();
                entity.Property(a => a.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<WeightUnit>(entity =>
            {
                entity.HasIndex(x => x.Id, "weightUnit_Status_Id").IsUnique();
                entity.Property(a => a.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasIndex(x => x.Id, "material_Id").IsUnique();
                entity.Property(a => a.Name).HasMaxLength(250);
                entity.Property(a => a.Code).HasMaxLength(50);
            });
        }
    }
}
