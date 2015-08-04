using HiyoshiCfhWeb.Models;
using XmlQuests = HiyoshiCfhWeb.XML.Quests;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System;

namespace HiyoshiCfhWeb.Controllers
{
    public class HeadquartersController : Controller
    {
        private string XmlPath
        {
            get
            {
                return Server.MapPath("~/XML/Quests.xml");
            }
        }

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Headquarters
        public ActionResult Index()
        {
            return View(db.Admirals);
        }

        public ActionResult Homeport(string id)
        {
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

        public ActionResult Quests(string id)
        {
            var serializer = new XmlSerializer(typeof(XmlQuests));
            XmlQuests questMaster;
            using (var stream = new FileStream(XmlPath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(stream))
                {
                    questMaster = (XmlQuests)serializer.Deserialize(reader);
                }
            }
            var quests = db.Quests.Where(x => x.Admiral.Name == id).OrderBy(x => x.QuestNo).ToList();
            foreach (var quest in quests)
            {
                // 必要条件での絞込
                var match = questMaster.Quest.Where(x => x.Compare(quest));
                if (match.Count() == 1)
                {
                    quest.Name += " " + match.First().Id;
                }
                else if (match.Count() > 1)
                {
                    foreach (var m in match)
                    {
                        quest.Name += " " + m.Id;
                    }
                }
            }
            return View(Tuple.Create(questMaster, quests));
        }
    }
}
