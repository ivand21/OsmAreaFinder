using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Models
{
    public static class PoiList
    {
        public static List<string> Poi { get; }

        static PoiList()
        {
            Poi = new List<string>
            {
                 "Supermarket",
                 "Piekarnia",
                 "Sklep odzieżowy",
                 "Sklep monopolowy",
                 "Fryzjer",
                 "Kiosk",
                 "Apteka",
                 "Sklep obuwniczy",
                 "Centrum handlowe",
                 "Pralnia",
                 "Bankomat",
                 "Bank",
                 "Bar",
                 "Przystanek autobusowy",
                 "Kawiarnia",
                 "Szpital",
                 "Siłownia",
                 "Lekarz",
                 "Szkoła",
                 "Stacja benzynowa",
                 "Przedszkole",
                 "Basen",
                 "Restauracja",
                 "Park",
                 "Parking",
                 "Fast food",
                 "Pub",
                 "Rynek",
                 "Kościół",
                 "Garaż"
            };

        }
    }
}