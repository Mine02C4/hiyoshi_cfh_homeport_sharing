using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HiyoshiCfhWeb.Controllers
{
    public class PluginController : Controller
    {
        // GET: Plugin
        public ActionResult Index()
        {
            var files = Directory.GetFiles(Server.MapPath("~/Content/Plugin")).Select(x => new FileInfo(x));
            return View(files);
        }
    }
}