using HiyoshiCfhWeb.Models;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace HiyoshiCfhWeb.Controllers
{
    public class HeadquartersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Headquarters
        public ActionResult Index()
        {
            return View(db.Admirals);
        }

        public ActionResult Homeport(string id)
        {
            System.Diagnostics.Trace.TraceInformation("name = {0}", id);
            var admiral = db.Admirals.Where(x => x.Name.Equals(id)).First();
            var results = db.Ships.Where(x => x.AdmiralId == admiral.AdmiralId).Join(db.ShipInfoes, ship => ship.ShipInfoId, shipInfo => shipInfo.ShipInfoId, (ship, shipInfo) => new
            {
                ship,
                shipInfo
            }).OrderByDescending(x => x.ship.Level);
            List<Ship> ships = new List<Ship>();
            foreach (var result in results)
            {
                result.ship.ShipInfo = result.shipInfo;
                ships.Add(result.ship);
            }
            admiral.Ships = ships;
            return View(admiral);
        }
    }
}