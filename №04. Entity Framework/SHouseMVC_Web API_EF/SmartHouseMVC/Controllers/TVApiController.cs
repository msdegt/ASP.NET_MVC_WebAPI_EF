using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartHouse;
using SmartHouseMVC.Models;

namespace SmartHouseMVC.Controllers
{
    public class TVApiController : ApiController
    {
        private DeviceContext db = new DeviceContext();
        
        // PUT: api/TVApi
        [Route("api/TVApi/{id}/{button}/{value}")]
        public string PutSet(int id, string button, int value)
        {
            SummaryTableDevices deviceFromSummaryTable = db.SumTableDevices.Find(id);
            
            if (button == "chan")
            {
                deviceFromSummaryTable.Tv.GoToChannel(value);
            }
            else if (button == "vol")
            {
                deviceFromSummaryTable.Tv.SetVolume(value);
            }
            db.SaveChanges();
            return "Устройство: " + deviceFromSummaryTable.Tv.Name + "<br>" + deviceFromSummaryTable.Tv.ToString();
        }

        // PUT: api/TVApi/button/id
        [Route("api/TVApi/{button}/{id}")]
        public string Put(int id, string button)
        {
            SummaryTableDevices deviceFromSummaryTable = db.SumTableDevices.Find(id);
            switch (button)
            {
                case "on":
                    deviceFromSummaryTable.Tv.On();
                    break;
                case "off":
                    deviceFromSummaryTable.Tv.Off();
                    break;
                case "nCh":
                    deviceFromSummaryTable.Tv.NextChannel();
                    break;
                case "eCh":
                    deviceFromSummaryTable.Tv.EarlyChannel();
                    break;
                case "prevCh":
                    deviceFromSummaryTable.Tv.PreviousChannel();
                    break;
                case "maxV":
                    deviceFromSummaryTable.Tv.MaxVolume();
                    break;
                case "minV":
                    deviceFromSummaryTable.Tv.MinVolume();
                    break;
                case "mute":
                    deviceFromSummaryTable.Tv.SetMute();
                    break;
                case "scan":
                    deviceFromSummaryTable.Tv.ChannelScan();
                    break;
                case "listChan": 
                    string str = deviceFromSummaryTable.Tv.ListChannel();
                    return str;
            }
            db.SaveChanges();
            return "Устройство: " + deviceFromSummaryTable.Tv.Name + "<br>" + deviceFromSummaryTable.Tv.ToString();
        }

        // DELETE: api/TVApi/5
        public void Delete(int id)
        {
            SummaryTableDevices deviceFromSummaryTable = db.SumTableDevices.Find(id);
            db.SumTableDevices.Remove(deviceFromSummaryTable);
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
