using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Models
{
    public class ServerResponse
    {
        public List<PolygonModel> Polygons { get; set; }

        public ServerResponse(List<PolygonModel> polygons)
        {
            Polygons = polygons;
        }
    }
    
}