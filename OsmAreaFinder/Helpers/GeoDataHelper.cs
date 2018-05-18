using DotSpatial.Data;
using DotSpatial.Positioning;
using DotSpatial.Topology;
using GeoAPI.CoordinateSystems;
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
        private static string CSV_FILE_DIR = Path.
            Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Models/shp");

        public static List<PolygonReply> ProcessRequest(UserRequest req)
        {
            var polygons = new List<PolygonReply>();

            IFeatureSet resultArea = CreateUserInputLayer(req.Lon, req.Lat, req.Radius);
            foreach (var f in req.Filters)
            {
                bool isMin = f.MinMaxType == "Minimum";
                var buffered = ApplyBuffer(GetShapefile(f.ObjectType), req.Radius, isMin);
                resultArea = Intersect(resultArea, buffered);
            }

            foreach (var f in resultArea.Features)
            {
                var item = new PolygonReply();
                foreach (var coord in f.Coordinates)
                {
                    item.Vertices.Add(new Coord() { Lon = coord.X, Lat = coord.Y });
                }
                if (item.Vertices.Count > 0)
                    polygons.Add(item);
            }
            return polygons;
        }

        public static IFeatureSet Intersect(IFeatureSet l1, IFeatureSet l2)
        {
            return l1.Intersection(l2, FieldJoinType.LocalOnly, null);
            //output.SaveAs("inter.shp", true);
            //return output;
        }

        public static IFeatureSet ApplyBuffer(string inpath, double distance, bool isMin)
        {
            IFeatureSet fs = FeatureSet.Open(inpath);      
            IFeatureSet bs = fs.Buffer(distance, false);
            return bs;
            //bs.SaveAs(@"Municipalities_Buffer.shp", true);
        }

        public static IFeatureSet CreateUserInputLayer(double lon, double lat, double radius)
        {
            FeatureSet fs = new FeatureSet(DotSpatial.Topology.FeatureType.Polygon);
            Feature center = new Feature(new Coordinate(lon, lat));

            var xy = new double[1] { radius };
            var z = new double[1] { 1.0 };
            var dist = new Distance(radius, DistanceUnit.Meters);
            
            var circle = center.Buffer(radius);
            fs.AddFeature(circle);
            return fs;
            //fs.SaveAs("outfile.shp", true);
        }

        public static string GetShapefile(string key)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                { "Sklep monopolowy", "alcohol.shp" },
                { "Bank", "bank.shp" },
                { "Bar", "bar.shp" },
                { "Przystanek autobusowy", "bus_stop.shp" },
                { "Kawiarnia", "cafe.shp" },
                { "Bankomat", "cash_machine.shp" },
                { "Kościół", "church.shp" },
                { "Przychodnia", "clinic.shp" },
                { "Sklep odzieżowy", "clothes.shp" },
                { "Fast food", "fast_food.shp" },
                { "Garaż", "garage.shp" },
                { "Siłownia", "gym.shp" },
                { "Fryzjer", "hairdresser.shp" },
                { "Szpital", "hospital.shp" },
                { "Przedszkole", "kindergarten.shp" },
                { "Kiosk", "kiosk.shp" },
                { "Rynek", "marketplace.shp" },
                { "Park", "park.shp" },
                { "Parking", "parking.shp" },
                { "Stacja benzynowa", "petrol_station.shp" },
                { "Apteka", "pharmacy.shp" },
                { "Plac zabaw", "playground.shp" },
                { "Pub", "pub.shp" },
                { "Restauracja", "restaurant.shp" },
                { "Szkoła", "school.shp" },
                { "Centrum handlowe", "shopping_centre.shp" },
                { "Supermarket", "supermarket.shp" },
                { "Basen", "swimming_pool.shp" },
            };

            try
            {
                return dict.First(i => i.Key == key).Value;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}