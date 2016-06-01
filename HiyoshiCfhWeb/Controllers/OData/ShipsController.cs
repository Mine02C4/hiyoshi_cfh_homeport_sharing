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

        // PUT: odata/Ships(5)
        public override IHttpActionResult Put([FromODataUri] int key, Delta<Ship> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var obj = dbs.Find(key);
            if (obj == null)
            {
                return NotFound();
            }

            int? sortieTag = obj.SortieTag;
            patch.Put(obj);
            obj.SortieTag = sortieTag;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(obj);
        }

        // PATCH: odata/Ships(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public override IHttpActionResult Patch([FromODataUri] int key, Delta<Ship> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var obj = dbs.Find(key);
            if (obj == null)
            {
                return NotFound();
            }

            int? sortieTag = obj.SortieTag;
            patch.Patch(obj);
            obj.SortieTag = sortieTag;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(obj);
        }
    }
}
