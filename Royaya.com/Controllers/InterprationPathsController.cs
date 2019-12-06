using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Royaya.com.Models;

namespace Royaya.com.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Royaya.com.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<InterprationPath>("InterprationPaths");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */

        [Authorize]
    public class InterprationPathsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CoreController core = new CoreController();

        // GET: odata/InterprationPaths
        [EnableQuery]
        public IQueryable<InterprationPath> GetInterprationPaths()
        {
            return db.InterprationPaths;
        }

        // GET: odata/InterprationPaths(5)
        [EnableQuery]
        public SingleResult<InterprationPath> GetInterprationPath([FromODataUri] int key)
        {
            return SingleResult.Create(db.InterprationPaths.Where(interprationPath => interprationPath.id == key));
        }

        // PUT: odata/InterprationPaths(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<InterprationPath> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InterprationPath interprationPath = await db.InterprationPaths.FindAsync(key);
            if (interprationPath == null)
            {
                return NotFound();
            }

            patch.Put(interprationPath);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterprationPathExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(interprationPath);
        }

        // POST: odata/InterprationPaths
        public async Task<IHttpActionResult> Post(InterprationPath interprationPath)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (interprationPath.Cost == null)
                return BadRequest("Cost can't be null");
            if (checkDuplicatedRecord(interprationPath.Cost))
                return BadRequest("There is another path with the same cost " + interprationPath.Cost);
            interprationPath.CreationDate = DateTime.Now;
            interprationPath.LastModificationDate = DateTime.Now;
            interprationPath.Creator = core.getCurrentUser().Id;
            interprationPath.Modifier = core.getCurrentUser().Id;
            db.InterprationPaths.Add(interprationPath);
            await db.SaveChangesAsync();

            return Created(interprationPath);
        }

        // PATCH: odata/InterprationPaths(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<InterprationPath> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            InterprationPath interprationPath = await db.InterprationPaths.FindAsync(key);
            if (interprationPath == null)
            {
                return NotFound();
            }
            //bool costUpdated = false;
            //foreach (var fieldname in patch.GetChangedPropertyNames())
            //{
            //    if (fieldname.Equals("Cost"))
            //    {
            //        costUpdated = true;
            //        break;
            //    }

            //}
            


            //patch.Patch(interprationPath);
            //if (costUpdated) {
            //    if (checkDuplicatedRecord(interprationPath.Cost))
            //        return BadRequest("There is another path with the same cost " + interprationPath.Cost);

            //}

            try
            {
                interprationPath.LastModificationDate = DateTime.Now;
                interprationPath.Modifier = core.getCurrentUser().Id;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterprationPathExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(interprationPath);
        }

        // DELETE: odata/InterprationPaths(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            InterprationPath interprationPath = await db.InterprationPaths.FindAsync(key);
            if (interprationPath == null)
            {
                return NotFound();
            }

            db.InterprationPaths.Remove(interprationPath);
            await db.SaveChangesAsync();

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

        private bool InterprationPathExists(int key)
        {
            return db.InterprationPaths.Count(e => e.id == key) > 0;
        }

        public bool checkDuplicatedRecord(double cost) {
            List<InterprationPath> items = db.InterprationPaths.Where(a => a.Cost.Equals(cost)).ToList();
            if (items.Count>0)
                return true;
            return false;
        }
    }
}
