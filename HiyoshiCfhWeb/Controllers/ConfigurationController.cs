using HiyoshiCfhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;
using System.Web;
using System.Web.Mvc;

namespace HiyoshiCfhWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConfigurationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Configuration
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View(db.Users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deduplication()
        {
            var Admirals = db.Admirals;
            var duplication = db.Ships.GroupBy(ship => new { ship.AdmiralId, ship.ShipId }).Select(x => new
            {
                AdmiralId = x.Key.AdmiralId,
                ShipId = x.Key.ShipId,
                Count = x.Count()
            }).Where(x => x.Count > 1).ToList();
            if (duplication.Count() > 0)
            {
                foreach (var item in duplication)
                {
                    var ships = db.Ships.Where(x => x.AdmiralId == item.AdmiralId && x.ShipId == item.ShipId)
                        .OrderByDescending(x => x.ShipUid).Take(duplication.Count() - 1);
                    db.Ships.RemoveRange(ships);
                }
            }
            db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSortieTags()
        {
            var LastEvent = Event.Events.Last();
            var taggedShips = db.Ships.Where(x => x.SortieTag.HasValue && x.SortieTag > 0).ToList();
            foreach (var ship in taggedShips)
            {
                var tagRecord = new SortieTagRecord
                {
                    ShipUid = ship.ShipUid,
                    EventId = LastEvent.Id,
                    SortieTagId = ship.SortieTag.Value
                };
                db.SortieTagRecords.AddOrUpdate(tagRecord);
            }
            db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
