using HiyoshiCfhWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace HiyoshiCfhWeb.Controllers
{
    [Authorize]
    public class ImportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Materials()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Materials(HttpPostedFileWrapper importFile)
        {
            if (importFile != null)
            {
                var filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + importFile.FileName);
                importFile.SaveAs(filename);
                TempData["filename"] = filename;
                return RedirectToAction("MaterialsProc");
            }
            return View();
        }

        public ActionResult MaterialsProc(string id)
        {
            string filename = TempData["filename"] as string;
            if (filename == null)
            {
                return RedirectToAction("Materials");
            }
            else if (id == null)
            {
                TempData.Keep();
                return View(Tuple.Create(0, 0, filename, false));
            }
            else
            {
                var strs = id.Split('-');
                int start, end;
                if (Int32.TryParse(strs[0], out start) && Int32.TryParse(strs[1], out end) && end > start)
                {
                    var uid = User.Identity.GetUserId();
                    var admiral = db.Admirals.Where(x => x.UserId == uid).First();
                    var connection = new SQLiteConnection();
                    connection.ConnectionString = string.Format("Data Source={0};Version=3;", filename);
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM Resources LIMIT @offset, @limit";
                        command.Parameters.AddWithValue("@offset", start);
                        command.Parameters.AddWithValue("@limit", end - start);
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var record = new MaterialRecord();
                            record.TimeUtc = TimeZoneInfo.ConvertTimeToUtc(reader.GetDateTime(1), TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"));
                            record.Type = (MaterialType)reader.GetInt32(2);
                            record.Value = reader.GetInt32(3);
                            record.AdmiralId = admiral.AdmiralId;
                            db.MaterialRecords.Add(record);
                        }
                        db.SaveChanges();
                    }
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM Resources";
                        var reader = command.ExecuteReader();
                        reader.Read();
                        var size = reader.GetInt32(0);
                        if (size <= end)
                        {
                            return View(Tuple.Create(start, size, filename, true));
                        }
                        else
                        {
                            TempData.Keep();
                            return View(Tuple.Create(start, end, filename, false));
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Materials");
                }
            }
        }
    }
}