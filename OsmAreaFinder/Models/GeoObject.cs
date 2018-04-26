using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Models
{
    public class GeoObject
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string Name { get; set; }

        public GeoObject(double lat, double lon)
        {
            Lon = lon;
            Lat = lat;
        }
    }
}