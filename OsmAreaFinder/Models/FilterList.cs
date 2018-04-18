using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OsmAreaFinder.Models
{
    public class FilterList
    {
        public List<Filter> Filters { get; set; }

        public FilterList()
        {
            Filters = new List<Filter>();
        }
    }
}