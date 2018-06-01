using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Models
{
    public static class PoiList
    {
        public static Dictionary<string, string> Poi { get; }

        static PoiList()
        {
            Poi = new Dictionary<string, string>()
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
        }
        
        public static string GetShapefile(string key)
        {
            try
            {
                return Poi.First(i => i.Key == key).Value;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
