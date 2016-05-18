using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartHouse;
using SmartHouseMVC.Models;

namespace SmartHouseMVC.Controllers
{
    public class SmartHouseController : Controller
    {
        IRateOfOpening r;
        private DeviceContext db = new DeviceContext();

        public ActionResult Index()
        {
            SelectListItem[] devicesList = new SelectListItem[5];
            devicesList[0] = new SelectListItem { Text = "Телевизор", Value = "Tv", Selected = true };
            devicesList[1] = new SelectListItem { Text = "Холодильник", Value = "Ref" };
            devicesList[2] = new SelectListItem { Text = "Жалюзи", Value = "Shut" };
            devicesList[3] = new SelectListItem { Text = "Система капельного полива", Value = "Ws" };
            devicesList[4] = new SelectListItem { Text = "Бойлер", Value = "Boiler" };
            ViewBag.DevicesList = devicesList;
            ViewBag.Title = "Умный дом";
            List<Device> dev = GetDevices(db);

            return View(dev);
        }

        public static List<Device> GetDevices(DeviceContext db)
        {
            List<Device> dev = db.TVs.ToList().Cast<Device>().ToList();
            dev.AddRange(db.ReFs.ToList().Cast<Device>());
            dev.AddRange(db.WShutters.ToList().Cast<Device>());
            dev.AddRange(db.WSystems.ToList().Cast<Device>());
            dev.AddRange(db.Boilers.ToList().Cast<Device>());

            return dev;
        }

        public ActionResult Add(string deviceType)
        {

            Device newDevice;
            ICreate Create = new CreateObject();
            switch (deviceType)
            {
                default:
                    newDevice = Create.CreateTv();
                    db.TVs.Add((Television)newDevice);
                    break;
                case "Ref":
                    newDevice = Create.CreateRef();
                    db.ReFs.Add((Refrigerator)newDevice);
                    break;
                case "Boiler":
                    newDevice = Create.CreateBoiler();
                    db.Boilers.Add((Boiler)newDevice);
                    break;
                case "Shut":
                    newDevice = Create.CreateShut();
                    db.WShutters.Add((WindowShutters)newDevice);
                    break;
                case "Ws":
                    newDevice = Create.CreateWs();
                    db.WSystems.Add((WateringSystem)newDevice);
                    break;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Operation(string id, string command)
        {
            Device d = GetDevice(id);
            if (d == null)
            {
                return HttpNotFound();
            }
            if (d.Type == "Shut" || d.Type == "Ref")
            {
                r = (IRateOfOpening) d;
            }
            
            switch (command)
            {
                case "on":
                    d.On();
                    break;
                case "off":
                    d.Off();
                    break;
                case "open":
                    r.Open();
                    break;
                case "close":
                    r.Close();
                    break;                
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SetFreezeMode(string id, string frMode)
        {
            ISetFreezeMode f = (ISetFreezeMode)GetDevice(id);
            if (f == null)
            {
                return HttpNotFound();
            }
            switch (frMode)
            {
                case "Default":
                    break;
                case "LowFreeze":
                    f.SetLowFreeze();
                    break;
                case "ColderFreezing":
                    f.SetColderFreezing();
                    break;
                case "DeepFreeze":
                    f.SetDeepFreeze();
                    break;
                case "Defrost":
                    f.SetDefrost();
                    break;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SetTemperature(string id, double temp = 2)
        {
            ISetTemperature t = (ISetTemperature)GetDevice(id);
            if (t == null)
            {
                return HttpNotFound();
            }
            if (temp < 2 || temp > 15)
            {
                TempData["ErrorTemperature"] = "Ошибка! Недопустимое значение температуры.";
            }
            else
            {
                t.SetLevelTemperature(temp);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SetCustomMode(string id, double custom = 30)
        {
            ICustomMode c = (ICustomMode)GetDevice(id);
            if (c == null)
            {
                return HttpNotFound();
            }
            if (custom < 30 || custom > 90)
            {
                TempData["ErrorCustomMode"] = "Ошибка! Недопустимое значение температуры.";
            }
            else
            {
                c.SetCustomMode(custom);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SetTimeOfDayMode(string id, string timeMode)
        {
            ITimeOfDayMode time = (ITimeOfDayMode)GetDevice(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            Session["TimeMode"] = timeMode;
            switch (timeMode)
            {
                default:
                    time.SetMorningMode();
                    break;
                case "EveningMode":
                    time.SetEveningMode();
                    break;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SetModeHeating(string id, string h)
        {
            IModeHeating heating = (IModeHeating)GetDevice(id);
            if (heating == null)
            {
                return HttpNotFound();
            }
            Session["ModeHeating"] = h;
            switch (h)
            {
                default:
                    heating.SetMaxMode();
                    break;
                case "MinMode":
                    heating.SetMinMode();
                    break;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EnterLevel(string id, int level = 0)
        {
            IEnterLevel l = (IEnterLevel)GetDevice(id);
            if (l == null)
            {
                return HttpNotFound();
            }
            Session["Level"] = level; 
            if (level < 0 || level > 100)
            {
                TempData["ErrorSoilMoisture"] = "Ошибка! Недопустимое значение уровня влажности почвы."; 
            }
            else
            {
                l.EnterLevel(level);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            Device d = GetDevice(id);
            if (d == null)
            {
               return HttpNotFound();
            }
            switch (d.Type)
            {
                case "Tv":
                    db.TVs.Remove((Television)d);
                    break;
                case "Ws":
                    db.WSystems.Remove((WateringSystem)d);
                    break;
                case "Boiler":
                    db.Boilers.Remove((Boiler)d);
                    break;
                case "Shut":
                    db.WShutters.Remove((WindowShutters)d);
                    break;
                case "Ref":
                    db.ReFs.Remove((Refrigerator)d);
                    break;
            }
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private Device GetDevice(string id)
        {
            string[] mass = id.Split('_');
            int idDev;
            if (!int.TryParse(mass[0], out idDev))
            {
                return null;
            }
            switch (mass[1])
            {
                case "Tv":
                    return db.TVs.Find(idDev);                    
                case "Ws":
                    return db.WSystems.Find(idDev);
                case "Boiler":
                    return db.Boilers.Find(idDev);
                case "Shut":
                    return db.WShutters.Find(idDev);
                case "Ref":
                    return db.ReFs.Find(idDev);
                default:
                    return null;
            }
        }
    }
}