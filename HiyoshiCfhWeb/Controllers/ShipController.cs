using HiyoshiCfhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HiyoshiCfhWeb.Controllers
{
    public class ShipController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ships
        public ActionResult Index(string id)
        {
            var shipInfo = db.ShipInfoes.Where(x => x.Name == id).First();
            var ships = db.Ships.Where(x => x.ShipInfoId == shipInfo.ShipInfoId).OrderByDescending(x => x.Exp).ToList();
            return View(Tuple.Create(shipInfo, ships));
        }
    }
}
