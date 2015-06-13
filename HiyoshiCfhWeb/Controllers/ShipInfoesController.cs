using HiyoshiCfhWeb.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;

namespace HiyoshiCfhWeb.Controllers
{
    [Authorize]
    public class ShipInfoesController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/ShipInfoes
        [EnableQuery]
        public IQueryable<ShipInfo> GetShipInfoes()
        {
            return db.ShipInfoes;
        }

        // GET: odata/ShipInfoes(5)
        [EnableQuery]
        public SingleResult<ShipInfo> GetShipInfo([FromODataUri] int key)
        {
            return SingleResult.Create(db.ShipInfoes.Where(ShipInfo => ShipInfo.ShipInfoId == key));
        }

        // PUT: odata/ShipInfoes(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<ShipInfo> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShipInfo ShipInfo = db.ShipInfoes.Find(key);
            if (ShipInfo == null)
            {
                return NotFound();
            }

            patch.Put(ShipInfo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipInfoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(ShipInfo);
        }

        // POST: odata/ShipInfoes
        public IHttpActionResult Post(ShipInfo ShipInfo)
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
                    if (!ShipInfoExists(ShipInfo.ShipInfoId))
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

        // PATCH: odata/ShipInfoes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<ShipInfo> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShipInfo ShipInfo = db.ShipInfoes.Find(key);
            if (ShipInfo == null)
            {
                return NotFound();
            }

            patch.Patch(ShipInfo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipInfoExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(ShipInfo);
        }

        // DELETE: odata/ShipInfoes(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            ShipInfo ShipInfo = db.ShipInfoes.Find(key);
            if (ShipInfo == null)
            {
                return NotFound();
            }

            db.ShipInfoes.Remove(ShipInfo);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShipInfoExists(int key)
        {
            return db.ShipInfoes.Count(e => e.ShipInfoId == key) > 0;
        }
    }
}