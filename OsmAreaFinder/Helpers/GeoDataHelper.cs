using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Positioning;
using DotSpatial.Projections;
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

        public static List<PolygonModel> ProcessRequest(UserRequest req)
        {
            var polygons = new List<PolygonModel>();

            IFeatureSet resultArea = CreateUserInputLayer(req.Lon, req.Lat, req.Radius);
            var c = Complement(resultArea);

            foreach (var f in req.Filters)
            {
                bool isMin = f.MinMaxType == "Minimum";
                var buffered = ApplyBuffer(f.ObjectType, f.Distance, isMin);
                resultArea = Intersect(resultArea, buffered);
            }

            foreach (var s in resultArea.ShapeIndices)
            {
                foreach (var part in s.Parts)
                {
                    var item = new PolygonModel() { Vertices = new List<Coord>() };

                    for (int i = part.StartIndex * 2; i <= part.EndIndex * 2; i += 2)
                    {
                        Coord point = new Coord();
                        point.Lon = part.Vertices[i];
                        point.Lat = part.Vertices[i + 1];
                        item.Vertices.Add(point);
                    }
                    polygons.Add(item);
                }
            }
            return polygons;
        }

        public static IFeatureSet Intersect(IFeatureSet l1, IFeatureSet l2)
        {
            var p1 = l1.Projection;
            var p2 = l2.Projection;
            var output = l1.Intersection(l2, FieldJoinType.LocalOnly, null);
            //output.Projection = ProjectionInfo.FromEpsgCode(3857);
            output.SaveAs("C:/test/inter.shp", true);
            //return l1.Intersection(l2, FieldJoinType.LocalOnly, null);
            return output;
        }

        public static IFeatureSet Complement(IFeatureSet l)
        {
            IFeatureSet fs = FeatureSet.Open(CSV_FILE_DIR + "/all.shp");
            var world = fs.Features[0];
            foreach (var ft in l.Features)
            {
                world = world.Difference(ft);
            }
            fs.Features[0] = world;
            fs.SaveAs("c:/test/compl.shp", true);
            return fs;
        }

        // metric coordinates
        public static IFeatureSet ApplyBuffer(string filter, double distance, bool isMin)
        {
            string filename = GetShapefile(filter);
            IFeatureSet fs = FeatureSet.Open(CSV_FILE_DIR + '/' + filename);
            IFeatureSet bs = fs.Buffer(distance, false);
            bs.SaveAs(@"C:/test/buf.shp", true);
            return bs;
        }

        // metric coordinates
        public static IFeatureSet CreateUserInputLayer(double lon, double lat, double radius)
        {
            FeatureSet infs = new FeatureSet(DotSpatial.Topology.FeatureType.Point);
            infs.Projection = KnownCoordinateSystems.Projected.WorldSpheroid.Mercatorsphere;
            Feature center = new Feature(new Coordinate(lon, lat));
            infs.AddFeature(center);

            var fs = infs.Buffer(radius, false);

            fs.SaveAs("C:/test/usr.shp", true);
            return fs;
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