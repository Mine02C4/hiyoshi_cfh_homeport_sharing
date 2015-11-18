using HiyoshiCfhWeb.Models;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;

namespace HiyoshiCfhWeb.Controllers.OData
{
    [Authorize]
    public class QuestsController : ODataControllerBase<Quest>
    {
        public QuestsController()
            : base()
        {
            dbs = db.Quests;
            updateTimeStamp = x =>
            {
                var admiral = db.Admirals.Where(y => y.AdmiralId == x.AdmiralId).FirstOrDefault();
                if (admiral != null)
                {
                    admiral.UpdateQuestsUtc = DateTime.UtcNow;
                }
            };
        }

        // GET: odata/Quests(5)/Admiral
        [EnableQuery]
        public SingleResult<Admiral> GetAdmiral([FromODataUri] int key)
        {
            return SingleResult.Create(db.Quests.Where(m => m.QuestId == key).Select(m => m.Admiral));
        }
    }
}
