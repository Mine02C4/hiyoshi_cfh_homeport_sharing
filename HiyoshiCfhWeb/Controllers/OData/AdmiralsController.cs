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
    public class AdmiralsController : ODataControllerBase<Admiral>
    {
        public AdmiralsController()
            : base()
        {
            dbs = db.Admirals;
        }

        // PUT: odata/Admirals(5)
        public override IHttpActionResult Put([FromODataUri] int key, Delta<Admiral> patch)
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
                if (!Exists(key))
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
        public override IHttpActionResult Post(Admiral Admiral)
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
            Admiral.UpdateUtc = DateTime.UtcNow;
            db.Admirals.Add(Admiral);
            db.SaveChanges();
            return Created(Admiral);
        }

        // PATCH: odata/Admirals(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public override IHttpActionResult Patch([FromODataUri] int key, Delta<Admiral> patch)
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
                if (!Exists(key))
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
        public override IHttpActionResult Delete([FromODataUri] int key)
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
    }
}
