using OsmAreaFinder.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Helpers
{
    public static class GeoDataHelper
    {
        private static int R = 6378137; // Earth radius
        private static Dictionary<string, string> FilesForPoi;

        public static bool ValidatePoint(GeoObject point, IList<Filter> filters)
        {
            foreach (var filter in filters)
            {
                var path = "Models/csv/alcohol.csv";
                string line = "";

                using (StreamReader sr = new StreamReader(path))
                {
                    bool isPointValid = false;
                    while (line != null)
                    {
                        line = sr.ReadLine();
                        try
                        {
                            var values = line.Split(';');
                            double lat = Double.Parse(values[1], CultureInfo.InvariantCulture);
                            double lon = Double.Parse(values[2], CultureInfo.InvariantCulture);
                            GeoObject obj = new GeoObject(lat, lon);
                            var dist = GetDistance(point, obj);
                            if (dist < filter.Distance)
                            {
                                isPointValid = true;
                                break;
                            }
                        }
                        catch (Exception) { }
                    }
                    if (!isPointValid) return false;
                }

            }
            return true;
        }

        private static double Radian(double x)
        {
            return x * Math.PI / 180;
        }

        public static double GetDistance(GeoObject g1, GeoObject g2)
        {           
            var dLat = Radian(g2.Lat - g1.Lat);
            var dLon = Radian(g2.Lon - g1.Lon);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(Radian(g1.Lat)) * Math.Cos(Radian(g2.Lat)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

    }
}