using HiyoshiCfhWeb.Models;
using System.Data.Entity.Infrastructure;
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
        }
    }
}
