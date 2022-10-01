using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
  [Authorize]
  [RoutePrefix("MapView")]
  public class MapViewController : Controller
    {
        // GET: MapView
        public ActionResult Index()
        {
            return View();
        }

    public ActionResult SetLoc() {

      return View();
      }
    }
}