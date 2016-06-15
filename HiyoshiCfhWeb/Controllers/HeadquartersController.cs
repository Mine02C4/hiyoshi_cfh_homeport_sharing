using HiyoshiCfhWeb.Extensions;
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
        public static string XmlVirtualPath
        {
            get
            {
                return "~/XML/Quests.xml";
            }
        }

        private string XmlPath
        {
            get
            {
                return Server.MapPath(XmlVirtualPath);
            }
        }

        private ApplicationDbContext db = new ApplicationDbContext();
        private Dictionary<string, List<XML.Quest>> retrietingEdge = null;

        public static List<Tuple<string, string>> subPages = new List<Tuple<string, string>> {
            Tuple.Create("母港", ""),
            Tuple.Create("任務進捗", "Quests"),
            Tuple.Create("イベント", "Event"),
            Tuple.Create("資材統計", "Materials"),
            Tuple.Create("近代化改修", "Modernization"),
            Tuple.Create("レベリング", "Leveling"),
            Tuple.Create("保有装備", "Equipments"),
        };

        // GET: Headquarters
        public ActionResult Index()
        {
            return View(db.Admirals);
        }

        public ActionResult Homeport(string id)
        {
            var admiral = db.Admirals.AsNoTracking().Where(x => x.Name.Equals(id)).First();            
            var ships = db.Ships.AsNoTracking().Include("ShipInfo.ShipType").Include("SortieTagRecords").Where(x => x.AdmiralId == admiral.AdmiralId).ToArray();
            db.Configuration.LazyLoadingEnabled = false;
            admiral.Ships = ships;
            return View(admiral);
        }

        /// <summary>
        /// 任務進捗の表示。任務進捗推論も実装。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            var admiral = db.Admirals.Where(x => x.Name.Equals(id)).First();
            var quests = db.Quests.Where(x => x.AdmiralId == admiral.AdmiralId)
                .OrderBy(x => x.QuestNo).ToList();
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

            // ウィークリーはデイリーがトリガーのため、デイリーをリセットされると、
            // 表示状況からは決定論的に進捗を求められない。
            // そのため、Bw9、Bw7が完了しない前提で、未完了を初期値として、
            // 表示された任務から依存するものを完了済みにするアプローチをとる。
            Action<XML.Quest> fdig = null;
            fdig = qid =>
            {
                qid.State = XML.QuestState.Achieved;
                foreach (var item in qid.Dependency)
                {
                    var q = questMaster.Quest.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (q != null && qid.Type == q.Type)
                    {
                        fdig(q);
                    }
                }
            };
            foreach (var quest in questMaster.Quest
                .Where(x => x.Type == XML.Type.weekly && x.Category == XML.Category.sortie))
            {
                quest.State = XML.QuestState.Invisible;
            }
            foreach (var quest in quests
                .Where(x => x.Type == QuestType.Weekly && x.Category == QuestCategory.Sortie))
            {
                var match = questMaster.Quest.Where(x => x.Compare(quest));
                foreach (var m in match)
                {
                    m.State = XML.QuestState.Visible;
                    fdig(m);
                }
            }
            // すべての任務に対して逆辺をたどり、未完了をマークしていく。
            Action<XML.Quest> dig = null;
            dig = qid =>
            {
                qid.State = XML.QuestState.Invisible;
                List<XML.Quest> qs;
                if (retrietingEdge.TryGetValue(qid.Id, out qs))
                {
                    foreach (var q in qs)
                    {
                        if (qid.Type == XML.Type.onetime || qid.Type == q.Type)
                        {
                            dig(q);
                        }
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
            // デイリーの完了状況からウィークリーの状態を再走査
            Action<XML.Quest> dfs = null;
            dfs = qid =>
            {
                if (qid.Dependency
                    .All(x =>
                    {
                        var q = questMaster.Quest.Where(y => y.Id == x.Id).FirstOrDefault();
                        if (q != null && q.State == XML.QuestState.Achieved)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }))
                {
                    qid.State = XML.QuestState.Achieved;
                    List<XML.Quest> qs;
                    if (retrietingEdge.TryGetValue(qid.Id, out qs))
                    {
                        foreach (var q in qs.Where(x => x.State == XML.QuestState.Invisible))
                        {
                            dfs(q);
                        }
                    }
                }
            };
            foreach (var quest in questMaster.Quest
                .Where(x => x.Type == XML.Type.daily && x.State == XML.QuestState.Achieved))
            {
                List<XML.Quest> qs;
                if (retrietingEdge.TryGetValue(quest.Id, out qs))
                {
                    foreach (var item in qs
                        .Where(x => x.Type == XML.Type.weekly && x.State == XML.QuestState.Invisible))
                    {
                        dfs(item);
                    }
                }
            }
            return View(Tuple.Create(admiral, questMaster, quests));
        }

        public ActionResult ShipType(string id, string param)
        {
            var admiral = db.Admirals.Where(x => x.Name.Equals(id)).First();
            var types = param.Split('_').ToList();
            var results = db.Ships
                .Where(x => x.AdmiralId == admiral.AdmiralId &&
                    types.Any(key => x.ShipInfo.ShipType.Name == key))
                .Join(
                    db.ShipInfoes,
                    ship => ship.ShipInfoId,
                    shipInfo => shipInfo.ShipInfoId,
                    (ship, shipInfo)
                    => new
                    {
                        ship,
                        shipInfo
                    })
                .OrderByDescending(x => x.ship.Level);
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

        public ActionResult Materials(string id, string type, string target, string range)
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
                else if (target == "bucket")
                {
                    material = Material.List.GetRange(5, 1);
                }
                else
                {
                    material = Material.List.GetRange(4, 4);
                }
                var records = db.MaterialRecords.Where(x => x.AdmiralId == admiral.AdmiralId);
                bool addCurrentValue = true;
                if (range != null && range == "event")
                {
                    var ev = Models.Event.Events.Last();
                    if (!ev.IsInDeployment)
                    {
                        addCurrentValue = false;
                    }
                    records = records.Where(x => x.TimeUtc > ev.StartTime.UtcDateTime && x.TimeUtc < ev.FinishTime.UtcDateTime);
                }
                else
                {
                    addCurrentValue = true;
                    DateTime startTime = DateTime.UtcNow.AddMonths(-1);
                    records = records.Where(x => x.TimeUtc > startTime && x.TimeUtc < DateTime.UtcNow);
                }
                var nlimit = 530;
                var obj =
                    material.Select(m =>
                    {
                        var count = records.Where(x => x.Type == m.Type).Count();
                        if (count <= nlimit)
                        {
                            return new
                            {
                                key = m.Name,
                                values = records.Where(x => x.Type == m.Type)
                                .OrderBy(x => x.TimeUtc)
                                .Select(x => new
                                {
                                    x.TimeUtc,
                                    x.Value
                                }).ToList()
                                .Select(x => new
                                {
                                    time = x.TimeUtc.UtcToJst().ToString("O"),
                                    value = x.Value
                                }).ToArray()
                            };
                        }
                        else
                        {
                            var values = records.Where(x => x.Type == m.Type)
                                .OrderBy(x => x.TimeUtc)
                                .Select(x => new
                                {
                                    x.TimeUtc,
                                    x.Value
                                })
                                .ToList().Where((x, i) => i % (count / nlimit + 1) == 0)
                                .Select(x => new
                                {
                                    time = x.TimeUtc.UtcToJst().ToString("O"),
                                    value = x.Value
                                }).ToList();
                            if (addCurrentValue)
                            {
                                values.Add(new
                                {
                                    time = DateTime.UtcNow.UtcToJst().ToString("O"),
                                    value = records.Where(x => x.Type == m.Type)
                                            .OrderByDescending(x => x.TimeUtc)
                                            .First().Value
                                }
                                );
                            }
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
                var records = db.MaterialRecords
                    .Where(x => x.AdmiralId == admiral.AdmiralId)
                    .OrderBy(x => x.TimeUtc);
                return View(Tuple.Create(admiral, records));
            }
        }

        public ActionResult Modernization(string id)
        {
            var admiral = db.Admirals.AsNoTracking().Where(x => x.Name.Equals(id)).First();
            var ships = db.Ships.AsNoTracking().Include("ShipInfo").Where(x => x.AdmiralId == admiral.AdmiralId).ToArray();
            db.Configuration.LazyLoadingEnabled = false;
            admiral.Ships = ships;
            return View(admiral);
        }

        public ActionResult Leveling(string id)
        {
            var admiral = db.Admirals.AsNoTracking().Where(x => x.Name.Equals(id)).First();
            var ships = db.Ships.AsNoTracking().Include("ShipInfo.ShipType").Where(x => x.AdmiralId == admiral.AdmiralId).ToArray();
            db.Configuration.LazyLoadingEnabled = false;
            admiral.Ships = ships;
            return View(admiral);
        }

        public ActionResult Equipments(string id)
        {
            var admiral = db.Admirals.Where(x => x.Name.Equals(id)).First();
            return View(admiral);
        }
    }
}
