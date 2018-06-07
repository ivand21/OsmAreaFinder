using DotSpatial.Analysis;
using DotSpatial.Data;
using DotSpatial.Positioning;
using DotSpatial.Projections;
using DotSpatial.Topology;
using GeoAPI.CoordinateSystems;
using OSGeo.OGR;
using OsmAreaFinder.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace OsmAreaFinder.Helpers
{
    public static class GeoDataHelper
    {
        private static string CSV_FILE_DIR = Path.
            Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Models/shp");
        private static string WORKSPACE_DIR = "C:/test/";

        public static string ProcessRequest(UserRequest req)
        {
            var workdir = CreateWorkspace();

            IFeatureSet resultArea = CreateUserInputLayer(req.Lon, req.Lat, req.Radius, workdir);
            var c = Complement(resultArea, workdir);

            foreach (var f in req.Filters)
            {
                bool isMin = f.MinMaxType == "Minimum";
                var buffered = ApplyBuffer(f.ObjectType, f.Distance, isMin, workdir);
                resultArea = Intersect(resultArea, buffered, workdir);
            }

            resultArea.SaveAs(WORKSPACE_DIR + workdir + "/output.shp", true);
            var json = ToGeoJson(WORKSPACE_DIR + workdir + "/output.shp");
            CleanWorkspace(workdir);

            return json;
            //foreach (var s in resultArea.ShapeIndices)
            //{
            //    foreach (var part in s.Parts)
            //    {
            //        var item = new PolygonModel() { Vertices = new List<Coord>() };
            //        // TODO use Shoelace formula to determine if polygon is clockwise - if yes:
            //        // this is exterior polygon, else - interior polygon (a hole)

            //        for (int i = part.StartIndex * 2; i <= part.EndIndex * 2; i += 2)
            //        {
            //            Coord point = new Coord();
            //            point.Lon = part.Vertices[i];
            //            point.Lat = part.Vertices[i + 1];

            //            item.Vertices.Add(point);
            //        }
            //        polygons.Add(item);
            //    }
            //}
            //return polygons;
        }

        public static IFeatureSet Intersect(IFeatureSet l1, IFeatureSet l2, string workdir)
        {
            var p1 = l1.Projection;
            var p2 = l2.Projection;
            var output = l1.Intersection(l2, FieldJoinType.LocalOnly, null);
            //output.Projection = ProjectionInfo.FromEpsgCode(3857);
            for (int i = 0; ; i++)
            {
                if (!File.Exists(WORKSPACE_DIR + workdir + "/inter" + i + ".shp"))
                {
                    output.SaveAs(WORKSPACE_DIR + workdir + "/inter" + i + ".shp", true);
                    break;
                }
            }
            //return l1.Intersection(l2, FieldJoinType.LocalOnly, null);
            return output;
        }

        public static IFeatureSet Complement(IFeatureSet l, string workdir)
        {
            IFeatureSet fs = FeatureSet.Open(CSV_FILE_DIR + "/all.shp");
            var world = fs.Features[0];
            foreach (var ft in l.Features)
            {
                world = world.Difference(ft);
            }
            fs.Features[0] = world;
            fs.SaveAs(WORKSPACE_DIR + workdir + "/compl.shp", true);
            return fs;
        }

        // metric coordinates
        public static IFeatureSet ApplyBuffer(string filter, double distance, bool isMin, string workdir)
        {
            string filename = PoiList.GetShapefile(filter);
            IFeatureSet fs = FeatureSet.Open(CSV_FILE_DIR + '/' + filename);
            IFeatureSet bs = fs.Buffer(distance, false);

            if (isMin)
            {
                bs = Complement(bs, workdir);
            }

            for (int i = 0; ; i++)
            {
                if (!File.Exists(WORKSPACE_DIR + workdir + "/buf" + i + ".shp"))
                {
                    bs.SaveAs(WORKSPACE_DIR + workdir + "/buf" + i + ".shp", true);
                    break;
                }
            }

            bs.SaveAs(WORKSPACE_DIR + workdir + "/buf.shp", true);
            return bs;
        }

        // metric coordinates
        public static IFeatureSet CreateUserInputLayer(double lon, double lat, double radius, string workdir)
        {
            FeatureSet infs = new FeatureSet(DotSpatial.Topology.FeatureType.Point);
            infs.Projection = KnownCoordinateSystems.Projected.WorldSpheroid.Mercatorsphere;
            DotSpatial.Data.Feature center = new DotSpatial.Data.Feature(new Coordinate(lon, lat));
            infs.AddFeature(center);

            var fs = infs.Buffer(radius, false);

            fs.SaveAs(WORKSPACE_DIR + workdir + "/usr.shp", true);
            return fs;
        }

        public static string ToGeoJson(string shpPath)
        {
            OSGeo.OGR.Ogr.RegisterAll();
            Driver drv = Ogr.GetDriverByName("ESRI Shapefile");

            using (var ds = drv.Open(shpPath, 0))
            {
                OSGeo.OGR.Layer layer = ds.GetLayerByIndex(0);

                OSGeo.OGR.Feature f;
                layer.ResetReading();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                while ((f = layer.GetNextFeature()) != null)
                {
                    var geom = f.GetGeometryRef();
                    if (geom != null)
                    {
                        var geometryJson = geom.ExportToJson(null);
                        sb.AppendLine(geometryJson);
                    }
                }

                return sb.ToString();
            }
        }

        public static void CleanWorkspace(string dir)
        {
            Directory.Delete(WORKSPACE_DIR + dir, true);
        }

        public static string CreateWorkspace()
        {
            Guid name = Guid.NewGuid();
            Directory.CreateDirectory(WORKSPACE_DIR + name);
            return name.ToString();
        }
    }
}