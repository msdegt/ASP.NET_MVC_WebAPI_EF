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
            Device d1 = Create.CreateTv();
            context.TVs.Add((Television)d1);
            Device d2 = Create.CreateRef();
            context.ReFs.Add((Refrigerator)d2);
            Device d3 = Create.CreateShut();
            context.WShutters.Add((WindowShutters)d3);
            Device d4 = Create.CreateWs();
            context.WSystems.Add((WateringSystem)d4);
            Device d5 = Create.CreateBoiler();
            context.Boilers.Add((Boiler)d5);
            context.SaveChanges(); // для того, что бы воводил на страницу по порядку

            context.SumTableDevices.Add(new SummaryTableDevices { Tv = (Television)d1, Id = 1, TypeOfDevice = "Tv" });
            context.SumTableDevices.Add(new SummaryTableDevices { Ref = (Refrigerator)d2, Id = 2, TypeOfDevice = "Ref" });
            context.SumTableDevices.Add(new SummaryTableDevices { WShutters = (WindowShutters)d3, Id = 3, TypeOfDevice = "Shut" });
            context.SumTableDevices.Add(new SummaryTableDevices { WSystem = (WateringSystem)d4, Id = 4, TypeOfDevice = "Ws" });
            context.SumTableDevices.Add(new SummaryTableDevices { Boiler = (Boiler)d5, Id = 5, TypeOfDevice = "Boiler" });

            context.SaveChanges();
        }
    }
}