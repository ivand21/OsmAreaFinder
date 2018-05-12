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

        [Display(Name = "Rodzaj filtru")]
        public string MinMaxType { get; set; }

        [Display(Name = "Odległość (metry)")]
        public double Distance { get; set; }

        public Filter(string type, string minmax, double dist)
        {
            ObjectType = type;
            MinMaxType = minmax;
            Distance = dist;
        }
    }
}