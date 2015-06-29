using HiyoshiCfhWeb.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;

namespace HiyoshiCfhWeb.Controllers
{
    [Authorize]
    public class AdmiralsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Admirals
        [EnableQuery]
        public IQueryable<Admiral> GetAdmirals()
        {
            return db.Admirals;
        }

        // GET: odata/Admirals(5)
        [EnableQuery]
        public SingleResult<Admiral> GetAdmiral([FromODataUri] int key)
        {
            return SingleResult.Create(db.Admirals.Where(Admiral => Admiral.AdmiralId == key));
        }

        // PUT: odata/Admirals(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Admiral> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Admiral Admiral = db.Admirals.Find(key);
            if (Admiral == null)
            {
                return NotFound();
            }
            if (Admiral.UserId != User.Identity.GetUserId())
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            patch.Put(Admiral);
            Admiral.UpdateUtc = DateTime.UtcNow;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmiralExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(Admiral);
        }

        // POST: odata/Admirals
        public IHttpActionResult Post(Admiral Admiral)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (db.Admirals.Where(x => x.MemberId == Admiral.MemberId).Count() > 0)
            {
                return Conflict();
            }
            Admiral.UserId = User.Identity.GetUserId();
            db.Admirals.Add(Admiral);
            db.SaveChanges();
            return Created(Admiral);
        }

        // PATCH: odata/Admirals(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Admiral> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Admiral Admiral = db.Admirals.Find(key);
            if (Admiral == null)
            {
                return NotFound();
            }
            if (Admiral.UserId != User.Identity.GetUserId())
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            patch.Patch(Admiral);
            Admiral.UpdateUtc = DateTime.UtcNow;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmiralExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(Admiral);
        }

        // DELETE: odata/Admirals(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Admiral Admiral = db.Admirals.Find(key);
            if (Admiral == null)
            {
                return NotFound();
            }
            if (Admiral.UserId != User.Identity.GetUserId())
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

            db.Admirals.Remove(Admiral);
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

        private bool AdmiralExists(int key)
        {
            return db.Admirals.Count(e => e.AdmiralId == key) > 0;
        }
    }
}
