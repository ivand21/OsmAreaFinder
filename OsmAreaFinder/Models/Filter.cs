using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Models
{
    public class Filter
    {
        [Display(Name = "Obiekt")]
        public string ObjectType { get; set; }
        [Display(Name = "Maksymalna odległość (metry)")]
        public double Distance { get; set; }
    }
}