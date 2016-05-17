using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SmartHouse;

namespace SmartHouseMVC.Models
{
    public class SummaryTableDevices
    {
        public int Id { get; set; }

        [Required]
        public string TypeOfDevice { get; set; }

        public int? TvId { get; set; }
        public virtual Television Tv { get; set; }

        public int? RefId { get; set; }
        public virtual Refrigerator Ref { get; set; }

        public int? WShuttersId { get; set; }
        public virtual WindowShutters WShutters { get; set; }

        public int? WSystemId { get; set; }
        public virtual WateringSystem WSystem { get; set; }

        public int? BoilerId { get; set; }
        public virtual Boiler Boiler { get; set; }
    }
}