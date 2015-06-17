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
            return View(db.ShipInfoes.Where(x => x.Name == id).First());
        }
    }
}