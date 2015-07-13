using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HiyoshiCfhWeb.Controllers
{
    public class PluginController : Controller
    {
        private string PluginPath
        {
            get
            {
                return Server.MapPath("~/Content/Plugin");
            }
        }

        private IEnumerable<FileInfo> PluginFiles
        {
            get
            {
                return Directory.GetFiles(PluginPath).Select(x => new FileInfo(x));
            }
        }

        // GET: Plugin
        public ActionResult Index()
        {
            return View(PluginFiles);
        }

        public ActionResult Zipped()
        {
            var memoryStream = new MemoryStream();
            using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create))
            {
                foreach (var file in PluginFiles)
                {
                    var entry = zip.CreateEntry(file.Name);
                    entry.LastWriteTime = TimeZoneInfo.ConvertTimeFromUtc(file.LastWriteTimeUtc,
                        TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"));
                    using (var writer = new BinaryWriter(entry.Open()))
                    {
                        writer.Write(System.IO.File.ReadAllBytes(file.FullName));
                    }
                }
            }
            return File(memoryStream.ToArray(), "application/zip", "HiyoshiCfhClient.zip");
        }
    }
}
