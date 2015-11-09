using HiyoshiCfhWeb.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;
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
        }
    }
}
