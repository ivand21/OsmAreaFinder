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
        private static string CSV_FILE_DIR = Path.
            Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Models/csv");


        public static bool ValidatePoint(UserRequest data)
        {
            foreach (var filter in data.Filters)
            {
                var path = Path.Combine(CSV_FILE_DIR, GetCsvFile(filter.ObjectType));
                string line = "";

                using (StreamReader sr = new StreamReader(path))
                {
                    bool isPointValid = false;
                    bool foundPointOverMinValue = false;
                    bool foundPointUnderMinValue = false;
                    string headerLine = sr.ReadLine();
                    while (line != null)
                    {
                        line = sr.ReadLine();
                        try
                        {
                            var values = line.Split(';');
                            double lat = Double.Parse(values[1], CultureInfo.InvariantCulture);
                            double lon = Double.Parse(values[2], CultureInfo.InvariantCulture);
                            GeoObject fileObj = new GeoObject(lat, lon);
                            GeoObject userObj = new GeoObject(data.Lat, data.Lon);
                            var dist = GetDistance(userObj, fileObj);


                            if (dist <= filter.Distance + data.Radius)
                            {
                                if (filter.MinMaxType.Equals("Maksimum"))
                                {
                                    isPointValid = true;
                                    break;
                                }
                                else if (filter.MinMaxType.Equals("Minimum"))
                                {
                                    foundPointUnderMinValue = true;
                                    isPointValid = false;
                                    break;
                                }
                            }
                            else if (!foundPointOverMinValue && filter.MinMaxType.Equals("Minimum") && (dist >= filter.Distance + data.Radius))
                            {
                                foundPointOverMinValue = true;
                            }
                        }
                        catch (Exception e) { Console.WriteLine(e.ToString()); }
                    }

                    if (!foundPointUnderMinValue && foundPointOverMinValue)
                        isPointValid = true;

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

        public static string GetCsvFile(string key)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                { "Sklep monopolowy", "alcohol.csv" },
                { "Bank", "bank.csv" },
                { "Bar", "bar.csv" },
                { "Przystanek autobusowy", "bus_stop.csv" },
                { "Kawiarnia", "cafe.csv" },
                { "Bankomat", "cash_machine.csv" },
                { "Kościół", "church.csv" },
                { "Przychodnia", "clinic.csv" },
                { "Sklep odzieżowy", "clothes.csv" },
                { "Fast food", "fast_food.csv" },
                { "Garaż", "garage.csv" },
                { "Siłownia", "gym.csv" },
                { "Fryzjer", "hairdresser.csv" },
                { "Szpital", "hospital.csv" },
                { "Przedszkole", "kindergarten.csv" },
                { "Kiosk", "kiosk.csv" },
                { "Rynek", "marketplace.csv" },
                { "Park", "park.csv" },
                { "Parking", "parking.csv" },
                { "Stacja benzynowa", "petrol_station.csv" },
                { "Apteka", "pharmacy.csv" },
                { "Plac zabaw", "playground.csv" },
                { "Pub", "pub.csv" },
                { "Restauracja", "restaurant.csv" },
                { "Szkoła", "school.csv" },
                { "Centrum handlowe", "shopping_centre.csv" },
                { "Supermarket", "supermarket.csv" },
                { "Basen", "swimming_pool.csv" },
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


        public static List<Poi> GetPoiListFromFilters(UserRequest data)
        {
            var PoiListFromFilters = new List<Poi>();

            foreach (var filter in data.Filters)
            {
                var path = Path.Combine(CSV_FILE_DIR, GetCsvFile(filter.ObjectType));
                string line = "";

                using (StreamReader sr = new StreamReader(path))
                {
                    string headerLine = sr.ReadLine();
                    while (line != null)
                    {
                        line = sr.ReadLine();
                        try
                        {
                            var values = line.Split(';');
                            long id = Int64.Parse(values[0]);
                            double lat = Double.Parse(values[1], CultureInfo.InvariantCulture);
                            double lon = Double.Parse(values[2], CultureInfo.InvariantCulture);
                            double radius = filter.Distance;
                            string filtername = filter.ObjectType;
                            string minmaxtype = filter.MinMaxType;
                            PoiListFromFilters.Add(new Poi(lon, lat, filtername, radius, id, minmaxtype));
                        }
                        catch (Exception e){ Console.WriteLine(e.ToString()); }
                    }
                }
                
            }
            return PoiListFromFilters;
        }
    }
}