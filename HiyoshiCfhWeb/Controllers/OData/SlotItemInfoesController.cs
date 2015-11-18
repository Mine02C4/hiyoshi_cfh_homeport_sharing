using HiyoshiCfhWeb.Models;
using System.Linq;
using System.Web.Http;

namespace HiyoshiCfhWeb.Controllers.OData
{
    [Authorize]
    public class SlotItemInfoesController : ODataControllerBase<SlotItemInfo>
    {
        public SlotItemInfoesController()
            : base()
        {
            dbs = db.SlotItemInfoes;
        }
    }
}
