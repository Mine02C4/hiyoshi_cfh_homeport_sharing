using HiyoshiCfhWeb.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.OData;

namespace HiyoshiCfhWeb.Controllers.OData
{
    [Authorize]
    public class MaterialRecordsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/MaterialRecords
        [EnableQuery]
        public IQueryable<MaterialRecord> GetMaterialRecords()
        {
            return db.MaterialRecords;
        }

        // GET: odata/MaterialRecords(5)
        [EnableQuery]
        public SingleResult<MaterialRecord> GetMaterialRecord([FromODataUri] int key)
        {
            return SingleResult.Create(db.MaterialRecords.Where(MaterialRecord => MaterialRecord.MaterialRecordId == key));
        }

        // PUT: odata/MaterialRecords(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<MaterialRecord> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MaterialRecord MaterialRecord = db.MaterialRecords.Find(key);
            if (MaterialRecord == null)
            {
                return NotFound();
            }

            patch.Put(MaterialRecord);
            MaterialRecord.TimeUtc = DateTime.UtcNow;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialRecordExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(MaterialRecord);
        }

        // POST: odata/MaterialRecords
        public IHttpActionResult Post(MaterialRecord MaterialRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            MaterialRecord.TimeUtc = DateTime.UtcNow;
            db.MaterialRecords.Add(MaterialRecord);
            db.SaveChanges();

            return Created(MaterialRecord);
        }

        // PATCH: odata/MaterialRecords(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<MaterialRecord> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MaterialRecord MaterialRecord = db.MaterialRecords.Find(key);
            if (MaterialRecord == null)
            {
                return NotFound();
            }

            patch.Patch(MaterialRecord);
            MaterialRecord.TimeUtc = DateTime.UtcNow;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaterialRecordExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(MaterialRecord);
        }

        // DELETE: odata/MaterialRecords(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            MaterialRecord MaterialRecord = db.MaterialRecords.Find(key);
            if (MaterialRecord == null)
            {
                return NotFound();
            }

            db.MaterialRecords.Remove(MaterialRecord);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/MaterialRecords(5)/Admiral
        [EnableQuery]
        public SingleResult<Admiral> GetAdmiral([FromODataUri] int key)
        {
            return SingleResult.Create(db.MaterialRecords.Where(m => m.MaterialRecordId == key).Select(m => m.Admiral));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MaterialRecordExists(int key)
        {
            return db.MaterialRecords.Count(e => e.MaterialRecordId == key) > 0;
        }
    }
}
