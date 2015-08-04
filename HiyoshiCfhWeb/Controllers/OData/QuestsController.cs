using HiyoshiCfhWeb.Models;
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
    public class QuestsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Quests
        [EnableQuery]
        public IQueryable<Quest> GetQuests()
        {
            return db.Quests;
        }

        // GET: odata/Quests(5)
        [EnableQuery]
        public SingleResult<Quest> GetQuest([FromODataUri] int key)
        {
            return SingleResult.Create(db.Quests.Where(quest => quest.QuestId == key));
        }

        // PUT: odata/Quests(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Quest> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Quest quest = db.Quests.Find(key);
            if (quest == null)
            {
                return NotFound();
            }

            patch.Put(quest);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(quest);
        }

        // POST: odata/Quests
        public IHttpActionResult Post(Quest quest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quests.Add(quest);
            db.SaveChanges();

            return Created(quest);
        }

        // PATCH: odata/Quests(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Quest> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Quest quest = db.Quests.Find(key);
            if (quest == null)
            {
                return NotFound();
            }

            patch.Patch(quest);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(quest);
        }

        // DELETE: odata/Quests(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Quest quest = db.Quests.Find(key);
            if (quest == null)
            {
                return NotFound();
            }

            db.Quests.Remove(quest);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Quests(5)/Admiral
        [EnableQuery]
        public SingleResult<Admiral> GetAdmiral([FromODataUri] int key)
        {
            return SingleResult.Create(db.Quests.Where(m => m.QuestId == key).Select(m => m.Admiral));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestExists(int key)
        {
            return db.Quests.Count(e => e.QuestId == key) > 0;
        }
    }
}
