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
    builder.EntitySet<DreamComment>("DreamComments");
    builder.EntitySet<Dream>("Dreams"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DreamCommentsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CoreController core = new CoreController();

        // GET: odata/DreamComments
        [EnableQuery]
        public IQueryable<DreamComment> GetDreamComments()
        {
            return db.DreamComments;
        }

        // GET: odata/DreamComments(5)
        [EnableQuery]
        public SingleResult<DreamComment> GetDreamComment([FromODataUri] int key)
        {
            return SingleResult.Create(db.DreamComments.Where(dreamComment => dreamComment.id == key));
        }

        // PUT: odata/DreamComments(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<DreamComment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DreamComment dreamComment = await db.DreamComments.FindAsync(key);
            if (dreamComment == null)
            {
                return NotFound();
            }

            patch.Put(dreamComment);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DreamCommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dreamComment);
        }

        // POST: odata/DreamComments
        public async Task<IHttpActionResult> Post(DreamComment dreamComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dreamComment.Text.Equals(null) || dreamComment.Text.Equals(""))
            {
                return BadRequest("You must specify the text");
            }

            if (dreamComment.DreamId == null)
            {
                return BadRequest("You must add dream Id");
            }
            Dream dream = db.Dreams.Where(a => a.id.Equals(dreamComment.DreamId)).FirstOrDefault();
            if (dream == null)
                return BadRequest("No matching dream!");

            dreamComment.Dream = dream;
            dreamComment.CreationDate = DateTime.Now;
            dreamComment.LastModificationDate = DateTime.Now;
            dreamComment.Creator = core.getCurrentUser().Id;
            dreamComment.Modifier = core.getCurrentUser().Id;
            db.DreamComments.Add(dreamComment);
            await db.SaveChangesAsync();

            return Created(dreamComment);
        }

        // PATCH: odata/DreamComments(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<DreamComment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DreamComment dreamComment = await db.DreamComments.FindAsync(key);
            if (dreamComment == null)
            {
                return NotFound();
            }

            patch.Patch(dreamComment);

            try
            {
                dreamComment.LastModificationDate = DateTime.Now;
                dreamComment.Modifier = core.getCurrentUser().Id;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DreamCommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dreamComment);
        }

        // DELETE: odata/DreamComments(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            DreamComment dreamComment = await db.DreamComments.FindAsync(key);
            if (dreamComment == null)
            {
                return NotFound();
            }

            db.DreamComments.Remove(dreamComment);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/DreamComments(5)/Dream
        [EnableQuery]
        public SingleResult<Dream> GetDream([FromODataUri] int key)
        {
            return SingleResult.Create(db.DreamComments.Where(m => m.id == key).Select(m => m.Dream));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DreamCommentExists(int key)
        {
            return db.DreamComments.Count(e => e.id == key) > 0;
        }
    }
}
