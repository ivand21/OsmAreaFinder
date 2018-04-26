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
                 "Centrum handlowe",
                 "Bankomat",
                 "Bank",
                 "Bar",
                 "Przystanek autobusowy",
                 "Kawiarnia",
                 "Szpital",
                 "Siłownia",
                 "Przychodnia",
                 "Szkoła",
                 "Stacja benzynowa",
                 "Przedszkole",
                 "Basen",
                 "Restauracja",
                 "Park",
                 "Plac zabaw",
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
