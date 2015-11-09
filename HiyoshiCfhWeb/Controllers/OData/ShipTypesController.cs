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
    public class ShipTypesController : ODataControllerBase<ShipType>
    {
        public ShipTypesController()
            : base()
        {
            dbs = db.ShipTypes;
        }

        // POST: odata/ShipTypes
        public override IHttpActionResult Post(ShipType shipType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _shipType = db.ShipTypes.Find(shipType.ShipTypeId);
            if (_shipType == null)
            {
                db.ShipTypes.Add(shipType);
                db.SaveChanges();
            }
            else
            {
                db.Entry(_shipType).State = EntityState.Detached;
                db.ShipTypes.Attach(shipType);
                db.Entry(shipType).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Exists(shipType.ShipTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Created(shipType);
        }
    }
}
