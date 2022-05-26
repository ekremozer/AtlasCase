using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtlasCase.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AtlasCase.Data.Core.EntityFramework
{
    public class EfContext : DbContext
    {
        private string ConnectionString { get; set; }
        public EfContext(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionString"];
        }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<WeightUnit> WeightUnits { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelBuilderHelper.OnModelCreating(modelBuilder);
        }

        public void DbCreator()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            BaseDataCreator.Create(this);
        }
    }
}
