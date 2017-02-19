using HiyoshiCfhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HiyoshiCfhWeb.Controllers
{
    public class EventController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Event/e2015summer
        public ActionResult e2015summer()
        {
            return View(db.Admirals.ToList());
        }

        // GET: Event/Summary/(Event ID)
        public ActionResult Summary(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ev = Event.Events.Where(x => x.Id == id).First();
            return View(ev);
        }
    }
}
