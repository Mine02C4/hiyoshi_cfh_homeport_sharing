using HiyoshiCfhWeb.Models;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;

namespace HiyoshiCfhWeb.Controllers.OData
{
    [Authorize]
    public class MaterialRecordsController : ODataControllerBase<MaterialRecord>
    {
        public MaterialRecordsController()
            : base()
        {
            dbs = db.MaterialRecords;
            updateOwnTimeStamp = x =>
            {
                x.TimeUtc = DateTime.UtcNow;
            };
        }

        // GET: odata/MaterialRecords(5)/Admiral
        [EnableQuery]
        public SingleResult<Admiral> GetAdmiral([FromODataUri] int key)
        {
            return SingleResult.Create(db.MaterialRecords.Where(m => m.MaterialRecordId == key).Select(m => m.Admiral));
        }
    }
}
