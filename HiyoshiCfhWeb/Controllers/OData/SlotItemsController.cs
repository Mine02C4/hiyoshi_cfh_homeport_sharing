using HiyoshiCfhWeb.Models;
using System.Linq;
using System.Web.Http;

namespace HiyoshiCfhWeb.Controllers.OData
{
    [Authorize]
    public class SlotItemsController : ODataControllerBase<SlotItem>
    {
        public SlotItemsController()
            : base()
        {
            dbs = db.SlotItems;
            detectDuplication = x =>
            {
                if (dbs.Where(y => y.AdmiralId == x.AdmiralId &&
                    y.Id == x.Id).Count() > 0)
                    return true;
                else
                    return false;
            };
        }
    }
}
