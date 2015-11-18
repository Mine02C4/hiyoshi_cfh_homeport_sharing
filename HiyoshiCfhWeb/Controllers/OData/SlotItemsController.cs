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
        }
    }
}
