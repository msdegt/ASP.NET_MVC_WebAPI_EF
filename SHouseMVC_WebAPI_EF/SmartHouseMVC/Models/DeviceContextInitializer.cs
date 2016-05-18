using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SmartHouse;

namespace SmartHouseMVC.Models
{
    public class DeviceContextInitializer : DropCreateDatabaseAlways<DeviceContext>
    {
        ICreate Create = new CreateObject();
        protected override void Seed(DeviceContext context)
        {
            context.TVs.Add(Create.CreateTv());
            context.ReFs.Add(Create.CreateRef());
            context.WShutters.Add(Create.CreateShut());
            context.WSystems.Add(Create.CreateWs());
            context.Boilers.Add(Create.CreateBoiler());

            context.SaveChanges();
        }
    }
}