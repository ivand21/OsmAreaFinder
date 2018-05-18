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
            //GeoDataHelper.CreateUserInputLayer(18.5416321, 54.4967745, 1);
            return View();
        }

        [HttpPost]
        public ActionResult Search(string filters)
        {
            var data = JsonConvert.DeserializeObject<UserRequest>(filters);
            var reply = GeoDataHelper.ProcessRequest(data);
            var r = Json(reply);
            return Json(reply);
        }

       
    }
}