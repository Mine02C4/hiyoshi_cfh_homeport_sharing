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
    public class ShipsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Ships
        [EnableQuery]
        public IQueryable<Ship> GetShips()
        {
            return db.Ships;
        }

        // GET: odata/Ships(5)
        [EnableQuery]
        public SingleResult<Ship> GetShip([FromODataUri] int key)
        {
            return SingleResult.Create(db.Ships.Where(Ship => Ship.ShipId == key));
        }

        // PUT: odata/Ships(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Ship> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Ship Ship = db.Ships.Find(key);
            if (Ship == null)
            {
                return NotFound();
            }

            patch.Put(Ship);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(Ship);
        }

        // POST: odata/Ships
        public IHttpActionResult Post(Ship Ship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Ships.Add(Ship);
            db.SaveChanges();
            return Created(Ship);
        }

        // PATCH: odata/Ships(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Ship> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Ship Ship = db.Ships.Find(key);
            if (Ship == null)
            {
                return NotFound();
            }

            patch.Patch(Ship);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(Ship);
        }

        // DELETE: odata/Ships(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Ship Ship = db.Ships.Find(key);
            if (Ship == null)
            {
                return NotFound();
            }

            db.Ships.Remove(Ship);
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

        private bool ShipExists(int key)
        {
            return db.Ships.Count(e => e.ShipId == key) > 0;
        }
    }
}
