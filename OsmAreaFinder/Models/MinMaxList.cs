using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Models
{
    public static class MinMaxList
    {
        public static List<string> MinMax { get; }

        static MinMaxList()
        {
            MinMax = new List<string>
            {
                "Minimum",
                "Maksimum"
            };
        }

    }
}