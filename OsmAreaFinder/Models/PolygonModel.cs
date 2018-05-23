using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Models
{
    public class PolygonModel
    {
        public List<Coord> Vertices { get; set; } = new List<Coord>();
    }
}