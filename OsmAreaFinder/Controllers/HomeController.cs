using Newtonsoft.Json;
using OsmAreaFinder.Helpers;
using OsmAreaFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OsmAreaFinder.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string filters)
        {
            var data = JsonConvert.DeserializeObject<UserRequest>(filters);
            var isValid = GeoDataHelper.ValidatePoint(data);

            return Json(new { Message = isValid });
        }

       
    }
}