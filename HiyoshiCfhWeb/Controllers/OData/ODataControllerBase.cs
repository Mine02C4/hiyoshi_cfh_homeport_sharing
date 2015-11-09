using HiyoshiCfhWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.OData;

namespace HiyoshiCfhWeb.Controllers
{
    /// <summary>
    /// ODataコントローラー向けの基底クラス。これを継承してコントローラーを作成する。
    /// </summary>
    /// <typeparam name="T">ODataで提供するモデル</typeparam>
    public abstract class ODataControllerBase<T> : ODataController where T : class
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        protected DbSet<T> dbs;
        protected Action<T> updateTimeStamp = (x) => { };
        protected Action<T> updateOwnTimeStamp = (x) => { };

        // GET: odata/xxx
        [EnableQuery]
        public IQueryable<T> Get()
        {
            return dbs;
        }

        // GET: odata/xxx(5)
        [EnableQuery]
        public SingleResult<T> Get([FromODataUri] int key)
        {
            return SingleResult.Create(new List<T> { dbs.Find(key) }.AsQueryable());
        }

        // PUT: odata/xxx(5)
        public virtual IHttpActionResult Put([FromODataUri] int key, Delta<T> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            T obj = dbs.Find(key);
            if (obj == null)
            {
                return NotFound();
            }

            patch.Put(obj);
            updateOwnTimeStamp(obj);
            updateTimeStamp(obj);

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

        // POST: odata/xxx
        public virtual IHttpActionResult Post(T obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            updateOwnTimeStamp(obj);
            updateTimeStamp(obj);
            dbs.Add(obj);
            db.SaveChanges();
            return Created(obj);
        }

        // PATCH: odata/xxx(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public virtual IHttpActionResult Patch([FromODataUri] int key, Delta<T> patch)
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

            patch.Patch(obj);
            updateOwnTimeStamp(obj);
            updateTimeStamp(obj);

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

        // DELETE: odata/xxx(5)
        public virtual IHttpActionResult Delete([FromODataUri] int key)
        {
            T obj = dbs.Find(key);
            if (obj == null)
            {
                return NotFound();
            }

            updateTimeStamp(obj);
            dbs.Remove(obj);
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

        protected bool Exists(int key)
        {
            return dbs.Find(key) != null;
        }
    }
}