using HiyoshiCfhWeb.Models;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;
using System.Web.OData;

namespace HiyoshiCfhWeb.Controllers
{
    [Authorize]
    public class ShipsController : ODataControllerBase<Ship>
    {
        public ShipsController()
            : base()
        {
            dbs = db.Ships;
            detectDuplication = x =>
            {
                if (dbs.Where(y => y.AdmiralId == x.AdmiralId &&
                    y.ShipId == x.ShipId).Count() > 0)
                    return true;
                else
                    return false;
            };
            trapUpdate = x =>
            {
                if (x.SortieTag.HasValue && x.SortieTag > 0 && Event.Events.Last().IsInDeployment)
                {
                    var tagRecord = new SortieTagRecord
                    {
                        ShipUid = x.ShipUid,
                        EventId = Event.Events.Last().Id,
                        SortieTagId = x.SortieTag.Value
                    };
                    db.SortieTagRecords.AddOrUpdate(tagRecord);
                    db.SaveChanges();
                }
            };
        }
    }
}
