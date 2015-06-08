using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using HiyoshiCfhWeb.Models;

namespace HiyoshiCfhWeb.Controllers
{
    /*
    このコントローラーのルートを追加するには、WebApiConfig クラスで追加の変更が必要になる場合があります。場合に応じてこれらのステートメントを WebApiConfig クラスの Register メソッドにマージしてください。OData URL は大文字と小文字が区別されるので注意してください。

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using HiyoshiCfhWeb.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ShipType>("ShipTypes");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ShipTypesController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/ShipTypes
        [EnableQuery]
        public IQueryable<ShipType> GetShipTypes()
        {
            return db.ShipTypes;
        }

        // GET: odata/ShipTypes(5)
        [EnableQuery]
        public SingleResult<ShipType> GetShipType([FromODataUri] int key)
        {
            return SingleResult.Create(db.ShipTypes.Where(shipType => shipType.ShipTypeId == key));
        }

        // PUT: odata/ShipTypes(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<ShipType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShipType shipType = db.ShipTypes.Find(key);
            if (shipType == null)
            {
                return NotFound();
            }

            patch.Put(shipType);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(shipType);
        }

        // POST: odata/ShipTypes
        public IHttpActionResult Post(ShipType shipType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ShipTypes.Add(shipType);
            db.SaveChanges();

            return Created(shipType);
        }

        // PATCH: odata/ShipTypes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<ShipType> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ShipType shipType = db.ShipTypes.Find(key);
            if (shipType == null)
            {
                return NotFound();
            }

            patch.Patch(shipType);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipTypeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(shipType);
        }

        // DELETE: odata/ShipTypes(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            ShipType shipType = db.ShipTypes.Find(key);
            if (shipType == null)
            {
                return NotFound();
            }

            db.ShipTypes.Remove(shipType);
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

        private bool ShipTypeExists(int key)
        {
            return db.ShipTypes.Count(e => e.ShipTypeId == key) > 0;
        }
    }
}
