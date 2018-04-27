using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Models
{
    public class UserRequest
    {
        public double Radius { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public List<Filter> Filters { get; set; }
    }
}