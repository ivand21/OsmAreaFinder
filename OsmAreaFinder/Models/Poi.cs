using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Models
{
    public class Poi
    {
        public double Lon { get; set; }
        public double Lat { get; set; }
        public string FilterName { get; set; }
        public double Radius { get; set; }
        public long Id { get; set; }
        public string MinMaxType { get; set; }

        public Poi(double lon, double lat, string filtername, double radius, long id, string minmaxtype)
        {
            Lon = lon;
            Lat = lat;
            FilterName = filtername;
            Radius = radius;
            Id = id;
            MinMaxType = minmaxtype;
        }

    }
}