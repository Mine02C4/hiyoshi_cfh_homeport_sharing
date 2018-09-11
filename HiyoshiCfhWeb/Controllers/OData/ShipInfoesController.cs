using HiyoshiCfhWeb.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using Microsoft.AspNet.OData;

namespace HiyoshiCfhWeb.Controllers
{
    [Authorize]
    public class ShipInfoesController : ODataControllerBase<ShipInfo>
    {
        public ShipInfoesController()
            : base()
        {
            dbs = db.ShipInfoes;
        }

        // POST: odata/ShipInfoes
        public override IHttpActionResult Post(ShipInfo ShipInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _ShipInfo = db.ShipInfoes.Find(ShipInfo.ShipInfoId);
            if (_ShipInfo == null)
            {
                db.ShipInfoes.Add(ShipInfo);
                db.SaveChanges();
            }
            else
            {
                db.Entry(_ShipInfo).State = EntityState.Detached;
                db.ShipInfoes.Attach(ShipInfo);
                db.Entry(ShipInfo).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(ShipInfo.ShipInfoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Created(ShipInfo);
        }

        // GET odata/ShipInfoes(5)/ShipType
        [EnableQuery]
        public SingleResult<ShipType> GetShipType([FromODataUri] int key)
        {
            var result = db.ShipInfoes.Where(m => m.ShipInfoId == key).Select(m => m.ShipType);
            return SingleResult.Create(result);
        }
    }
}
