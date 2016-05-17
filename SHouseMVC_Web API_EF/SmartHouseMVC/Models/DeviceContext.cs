using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SmartHouse;

namespace SmartHouseMVC.Models
{
    public class DeviceContext : DbContext
    {
        static DeviceContext()
        {
            System.Data.Entity.Database.SetInitializer<DeviceContext>(new DeviceContextInitializer());
        }

        public DbSet<SummaryTableDevices> SumTableDevices { get; set; }

        public DbSet<Television> TVs { get; set; }
        public DbSet<Refrigerator> ReFs { get; set; }
        public DbSet<WateringSystem> WSystems { get; set; }
        public DbSet<Boiler> Boilers { get; set; }
        public DbSet<WindowShutters> WShutters { get; set; }
    }
}