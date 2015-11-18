using HiyoshiCfhWeb.Models;
using System.Linq;
using System.Web.Http;

namespace HiyoshiCfhWeb.Controllers
{
    [Authorize]
    public class ShipsController : ODataControllerBase<Ship>
    {
        public ShipsController()
            : base()
        {
            dbs = db.Ships;
        }
    }
}
