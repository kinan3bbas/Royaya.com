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
    builder.EntitySet<Dream>("Dreams");
    builder.EntitySet<ApplicationUser>("Users"); 
    builder.EntitySet<InterprationPath>("InterprationPaths"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class DreamsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private CoreController core = new CoreController();

        // GET: odata/Dreams
        [EnableQuery]
        public IQueryable<Dream> GetDreams()
        {
            
            
            return db.Dreams.OrderByDescending(a=>a.CreationDate);

        }

        // GET: odata/Dreams
        
        [Route("GetDreamsInfo")]
        public String GetDreamsInfo()
        {
            return "Test";
        }



        // GET: odata/Dreams(5)
        [EnableQuery]
        public IHttpActionResult GetDream([FromODataUri] int key)
        {
            Dream dream = db.Dreams.Where(a => a.id == key).FirstOrDefault();
            if (!core.getCurrentUser().Id.Equals(dream.Creator)) {
                dream.numberOfViews++;
                db.Entry(dream).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Ok(dream);
        }

        // PUT: odata/Dreams(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Dream> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dream dream = await db.Dreams.FindAsync(key);
            if (dream == null)
            {
                return NotFound();
            }

            patch.Put(dream);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DreamExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(dream);
        }

        // POST: odata/Dreams
        public async Task<IHttpActionResult> Post(Dream dream)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (dream.Description == null || dream.Description.Equals(""))
                return BadRequest("Description can't be empty!");
            dream.Status ="Active";
            dream.CreationDate = DateTime.Now;
            dream.LastModificationDate = DateTime.Now;
            dream.Creator = core.getCurrentUser().Id;
            dream.Modifier = core.getCurrentUser().Id;
            dream.path = db.InterprationPaths.Where(a => a.Cost == 0).FirstOrDefault();
            dream.InterpretationStartDate = DateTime.Now;
            dream.CreatorFireBaseId = core.getCurrentUser().FireBaseId;
            db.Dreams.Add(dream);
            await db.SaveChangesAsync();

            return Created(dream);
        }

        

        // PATCH: odata/Dreams(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Dream> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dream dream = await db.Dreams.FindAsync(key);
            if (dream == null)
            {
                return NotFound();
            }

            bool interpretatorUpdated=false;
            bool pathUpdated=false;
            bool dreamExplained = false;
            bool userRating = false;
            foreach (var fieldname in patch.GetChangedPropertyNames())
            {
                if (fieldname.Equals("interpretatorId"))
                {
                    interpretatorUpdated = true;
                    
                }
                if (fieldname.Equals("pathId"))
                {
                    pathUpdated = true;

                }
                if (fieldname.Equals("Explanation"))
                {
                    dreamExplained = true;

                }
                if (fieldname.Equals("RatingDate"))
                {
                    dreamExplained = true;

                }



            }

            patch.Patch(dream);

            try
            {
                if (pathUpdated)
                {
                    InterprationPath path = db.InterprationPaths.Where(a => a.id.Equals(dream.pathId)).FirstOrDefault();
                    if (path == null)
                    {
                        return BadRequest("No matching path!");
                    }
                    dream.path = path;
                    dream.InterpretationStartDate = DateTime.Now;
                }
                if (interpretatorUpdated)
                {
                    ApplicationUser interpatator = db.Users.Where(a => a.Id.Equals(dream.interpretatorId)).FirstOrDefault();
                    if (interpatator == null)
                    {
                        return BadRequest("Not matching interpretator!");
                    }
                    dream.interpretator = interpatator;
                    dream.InterpreterFireBaseId = interpatator.FireBaseId;
                }
                if (dreamExplained)
                {
                    dream.ExplanationDate = DateTime.Now;
                    dream.Status = "Done";
                }
                if (userRating)
                {
                    if (!dream.Status.Equals("Done"))
                        return BadRequest("User can't rate interpreter until he explan the dream!");
                    if (dream.UserRating < 0 || dream.UserRating > 5)
                    {
                        return BadRequest("Rating can only be between 0 and 5");
                    }
                    dream.UserRating = dream.UserRating;
                    dream.RatingDate = DateTime.Now;
                }

                dream.LastModificationDate = DateTime.Now;
                dream.Modifier = core.getCurrentUser().Id;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DreamExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(dream);
        }

        // DELETE: odata/Dreams(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            Dream dream = await db.Dreams.FindAsync(key);
            if (dream == null)
            {
                return NotFound();
            }

            db.Dreams.Remove(dream);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Dreams(5)/interpretator
        [EnableQuery]
        public SingleResult<ApplicationUser> Getinterpretator([FromODataUri] int key)
        {
            return SingleResult.Create(db.Dreams.Where(m => m.id == key).Select(m => m.interpretator));
        }

        // GET: odata/Dreams(5)/path
        [EnableQuery]
        public SingleResult<InterprationPath> Getpath([FromODataUri] int key)
        {
            return SingleResult.Create(db.Dreams.Where(m => m.id == key).Select(m => m.path));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DreamExists(int key)
        {
            return db.Dreams.Count(e => e.id == key) > 0;
        }

    }
}
