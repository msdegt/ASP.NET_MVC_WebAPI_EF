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
        ISetChannel ch;
        ISetVolume v;
        IChannelSetup s;

        // PUT: api/TVApi
        [Route("api/TVApi/{id}/{button}/{value}")]
        public string PutSet(int id, string button, int value)
        {
            Device dev = db.TVs.Find(id);
            switch (button)
            {
                case "chan":
                    ch = (ISetChannel)dev;
                    ch.GoToChannel(value);
                    break;
                case "vol":
                    v = (ISetVolume)dev;
                    v.SetVolume(value);
                    break;
            }
            db.SaveChanges();
            return "Устройство: " + dev.Name + "<br>" + dev.ToString();
        }

        // PUT: api/TVApi/button/id
        [Route("api/TVApi/{button}/{id}")]
        public string Put(int id, string button)
        {
            Device dev = db.TVs.Find(id);
            if (dev != null)
            {
                ch = (ISetChannel)dev;
                v = (ISetVolume)dev;
                s = (IChannelSetup)dev;
                switch (button)
                {
                    case "on":
                        dev.On();
                        break;
                    case "off":
                        dev.Off();
                        break;
                    case "nCh":
                        ch.NextChannel();
                        break;
                    case "eCh":
                        ch.EarlyChannel();
                        break;
                    case "prevCh":
                        ch.PreviousChannel();
                        break;
                    case "maxV":
                        v.MaxVolume();
                        break;
                    case "minV":
                        v.MinVolume();
                        break;
                    case "mute":
                        v.SetMute();
                        break;
                    case "scan":
                        s.ChannelScan();
                        break;
                    case "listChan":
                        string str = s.ListChannel();
                        return str;
                }
            }
            
            db.SaveChanges();
            return "Устройство: " + dev.Name + "<br>" + dev.ToString();
        }

        // DELETE: api/TVApi/5
        public void Delete(int id)
        {
            Device dev = db.TVs.Find(id);
            db.TVs.Remove((Television)dev);
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
