using HiyoshiCfhWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace HiyoshiCfhWeb.Controllers
{
    public class MonitorController : Controller
    {
        private string XmlPath
        {
            get
            {
                return Server.MapPath(HeadquartersController.XmlVirtualPath);
            }
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Monitor
        public ActionResult Index()
        {
            var serializer = new XmlSerializer(typeof(XML.Quests));
            XML.Quests questMaster;
            using (var stream = new FileStream(XmlPath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(stream))
                {
                    questMaster = (XML.Quests)serializer.Deserialize(reader);
                }
            }
            var quests = db.Quests.ToList().Where(x =>
                questMaster.Quest.Where(y => y.Compare(x)).Count() != 1
            );
            return View(quests);
        }
    }
}
