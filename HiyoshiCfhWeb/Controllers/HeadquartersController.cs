﻿using HiyoshiCfhWeb.Extensions;
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
        private Dictionary<string, List<XML.Quest>> retrietingEdge = null;

        // GET: Headquarters
        public ActionResult Index()
        {
            return View(db.Admirals);
        }

        public ActionResult Homeport(string id)
        {
            var admiral = db.Admirals.Where(x => x.Name.Equals(id)).First();
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
            if (retrietingEdge == null)
            {
                retrietingEdge = new Dictionary<string, List<XML.Quest>>();
                foreach (var quest in questMaster.Quest)
                {
                    foreach (var item in quest.Dependency)
                    {
                        if (!retrietingEdge.ContainsKey(item.Id))
                        {
                            retrietingEdge.Add(item.Id, new List<XML.Quest>());
                        }
                        retrietingEdge[item.Id].Add(quest);
                    }
                }
            }
            Action<XML.Quest> dig = null;
            dig = qid =>
            {
                qid.State = XML.QuestState.Invisible;
                List<XML.Quest> qs;
                if (retrietingEdge.TryGetValue(qid.Id, out qs))
                {
                    foreach (var q in qs)
                    {
                        dig(q);
                    }
                }
            };
            foreach (var quest in quests)
            {
                var match = questMaster.Quest.Where(x => x.Compare(quest));
                foreach (var m in match)
                {
                    quest.Name += " " + m.Id;
                    quest.IsMatched = true;
                    dig(m);
                    m.State = XML.QuestState.Visible;
                }
            }
            return View(Tuple.Create(questMaster, quests, retrietingEdge));
        }

        public ActionResult ShipType(string id, string param)
        {
            var admiral = db.Admirals.Where(x => x.Name.Equals(id)).First();
            var types = param.Split('_').ToList();
            var results = db.Ships.Where(x => x.AdmiralId == admiral.AdmiralId && types.Any(key => x.ShipInfo.ShipType.Name == key))
                .Join(db.ShipInfoes, ship => ship.ShipInfoId, shipInfo => shipInfo.ShipInfoId, (ship, shipInfo) => new
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
            return View(Tuple.Create(admiral, ships, param.Replace("_", "+")));
        }

        public ActionResult Event(string id)
        {
            var admiral = db.Admirals.Where(x => x.Name.Equals(id)).First();
            return View(admiral);
        }

        public ActionResult Materials(string id, string type, string target)
        {
            var admiral = db.Admirals.Where(x => x.Name.Equals(id)).First();
            if (type != null && type == "json")
            {
                List<MaterialTuple> material;
                if (target == null)
                {
                    material = Material.List;
                }
                else if (target == "main")
                {
                    material = Material.List.GetRange(0, 4);
                }
                else
                {
                    material = Material.List.GetRange(4, 4);
                }
                var nlimit = 530;
                var obj =
                    material.Select(m =>
                    {
                        var count = db.MaterialRecords.Where(x => x.AdmiralId == admiral.AdmiralId && x.Type == m.Type).Count();
                        if (count <= nlimit)
                        {
                            return new
                            {
                                key = m.Name,
                                values = db.MaterialRecords.Where(x => x.AdmiralId == admiral.AdmiralId && x.Type == m.Type).OrderBy(x => x.TimeUtc)
                                    .Select(x => new
                                    {
                                        x.TimeUtc,
                                        x.Value
                                    }).ToList().Select(x => new
                                    {
                                        time = x.TimeUtc.UtcToJst().ToString("O"),
                                        value = x.Value
                                    }).ToArray()
                            };
                        }
                        else
                        {
                            var values = db.MaterialRecords.Where(x => x.AdmiralId == admiral.AdmiralId && x.Type == m.Type).OrderBy(x => x.TimeUtc)
                                    .Select(x => new
                                    {
                                        x.TimeUtc,
                                        x.Value
                                    }).ToList().Where((x, i) => i % (count / nlimit + 1) == 0).Select(x => new
                                    {
                                        time = x.TimeUtc.UtcToJst().ToString("O"),
                                        value = x.Value
                                    }).ToList();
                            values.Add(new
                                    {
                                        time = DateTime.UtcNow.UtcToJst().ToString("O"),
                                        value = db.MaterialRecords.Where(x => x.AdmiralId == admiral.AdmiralId && x.Type == m.Type).OrderByDescending(x => x.TimeUtc).First().Value
                                    }
                                );
                            return new
                            {
                                key = m.Name,
                                values = values.ToArray()
                            };
                        }
                    }).ToArray();
                var jsonResult = Json(obj, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 10240000;
                return jsonResult;
            }
            else
            {
                var records = db.MaterialRecords.Where(x => x.AdmiralId == admiral.AdmiralId).OrderBy(x => x.TimeUtc);
                return View(Tuple.Create(admiral, records));
            }
        }
    }
}
