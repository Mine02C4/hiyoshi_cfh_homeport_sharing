using HiyoshiCfhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
