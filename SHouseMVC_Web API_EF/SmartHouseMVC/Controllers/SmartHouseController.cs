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

            return View(db.SumTableDevices.ToList());
        }

        public ActionResult Add(string deviceType)
        {
            Device newDevice;
            SummaryTableDevices summaryTableDevices;
            ICreate Create = new CreateObject();
            
            switch (deviceType)
            {
                default:
                    newDevice = Create.CreateTv();
                    db.TVs.Add((Television)newDevice);
                    summaryTableDevices = new SummaryTableDevices { Tv = (Television)newDevice, TypeOfDevice = "Tv" };
                    break;
                case "Ref":
                    newDevice = Create.CreateRef();
                    db.ReFs.Add((Refrigerator)newDevice);
                    summaryTableDevices = new SummaryTableDevices { Ref = (Refrigerator)newDevice, TypeOfDevice = "Ref" };
                    break;
                case "Boiler":
                    newDevice = Create.CreateBoiler();
                    db.Boilers.Add((Boiler)newDevice);
                    summaryTableDevices = new SummaryTableDevices { Boiler = (Boiler)newDevice, TypeOfDevice = "Boiler" };
                    break;
                case "Shut":
                    newDevice = Create.CreateShut();
                    db.WShutters.Add((WindowShutters)newDevice);
                    summaryTableDevices = new SummaryTableDevices { WShutters = (WindowShutters)newDevice, TypeOfDevice = "Shut" };
                    break;
                case "Ws":
                    newDevice = Create.CreateWs();
                    db.WSystems.Add((WateringSystem)newDevice);
                    summaryTableDevices = new SummaryTableDevices { WSystem = (WateringSystem)newDevice, TypeOfDevice = "Ws" };
                    break;
            }

            db.SumTableDevices.Add(summaryTableDevices);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Operation(int id, string command)
        {
            SummaryTableDevices deviceFromSummaryTable = GetDeviceFromSummaryTable(id);
            if (deviceFromSummaryTable == null)
            {
                return HttpNotFound();
            }
            Device d = null;
            IRateOfOpening r = null;

            if (deviceFromSummaryTable.TypeOfDevice == "Shut")
            {
                r = deviceFromSummaryTable.WShutters;
                d = deviceFromSummaryTable.WShutters;
            }
            else if (deviceFromSummaryTable.TypeOfDevice == "Ref")
            {
                r = deviceFromSummaryTable.Ref;
                d = deviceFromSummaryTable.Ref;
            }
            else if (deviceFromSummaryTable.TypeOfDevice == "Tv")
            {
                d = deviceFromSummaryTable.Tv;
            }
            else if (deviceFromSummaryTable.TypeOfDevice == "Boiler")
            {
                d = deviceFromSummaryTable.Boiler;
            }
            else if (deviceFromSummaryTable.TypeOfDevice == "Ws")
            {
                d = deviceFromSummaryTable.WSystem;
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

        public ActionResult SetFreezeMode(int id, string frMode)
        {
            SummaryTableDevices deviceFromSummaryTable = GetDeviceFromSummaryTable(id);
            if (deviceFromSummaryTable == null)
            {
                return HttpNotFound();
            }
            ISetFreezeMode f = deviceFromSummaryTable.Ref;
            Session["FreezeMode"] = frMode;
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

        public ActionResult SetTemperature(int id, double temp = 2)
        {
            SummaryTableDevices deviceFromSummaryTable = GetDeviceFromSummaryTable(id);
            if (deviceFromSummaryTable == null)
            {
                return HttpNotFound();
            }
            ISetTemperature t = deviceFromSummaryTable.Ref;
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

        public ActionResult SetCustomMode(int id, double custom = 30)
        {
            SummaryTableDevices deviceFromSummaryTable = GetDeviceFromSummaryTable(id);
            if (deviceFromSummaryTable == null)
            {
                return HttpNotFound();
            }
            ICustomMode с = deviceFromSummaryTable.Boiler;
            if (custom < 30 || custom > 90)
            {
                TempData["ErrorCustomMode"] = "Ошибка! Недопустимое значение температуры.";
            }
            else
            {
                с.SetCustomMode(custom);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SetTimeOfDayMode(int id, string timeMode)
        {
            SummaryTableDevices deviceFromSummaryTable = GetDeviceFromSummaryTable(id);
            if (deviceFromSummaryTable == null)
            {
                return HttpNotFound();
            }
            ITimeOfDayMode time = deviceFromSummaryTable.WShutters;
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

        public ActionResult SetModeHeating(int id, string h)
        {
            SummaryTableDevices deviceFromSummaryTable = GetDeviceFromSummaryTable(id);
            if (deviceFromSummaryTable == null)
            {
                return HttpNotFound();
            }
            IModeHeating heating = deviceFromSummaryTable.Boiler;
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

        public ActionResult EnterLevel(int id, int level = 0)
        {
            SummaryTableDevices deviceFromSummaryTable = GetDeviceFromSummaryTable(id);
            if (deviceFromSummaryTable == null)
            {
                return HttpNotFound();
            }
            IEnterLevel l = deviceFromSummaryTable.WSystem;
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

        public ActionResult Delete(int id)
        {
            SummaryTableDevices deviceFromSummaryTable = GetDeviceFromSummaryTable(id);
            if (deviceFromSummaryTable == null)
            {
                return HttpNotFound();
            }
            db.SumTableDevices.Remove(deviceFromSummaryTable);
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

        private SummaryTableDevices GetDeviceFromSummaryTable(int? id)
        {
            SummaryTableDevices deviceFromSummaryTable = db.SumTableDevices.Find(id);
            if (id != null || deviceFromSummaryTable != null)
            {
                return deviceFromSummaryTable;
            }
            return null;
        }
    }
}