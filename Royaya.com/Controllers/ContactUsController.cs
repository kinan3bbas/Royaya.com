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
    builder.EntitySet<ContactUs>("ContactUs");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ContactUsController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CoreController core = new CoreController();

        // GET: odata/ContactUs
        [EnableQuery]
        public IQueryable<ContactUs> GetContactUs()
        {
            return db.ContactUss;
        }

        // GET: odata/ContactUs(5)
        [EnableQuery]
        public SingleResult<ContactUs> GetContactUs([FromODataUri] int key)
        {
            return SingleResult.Create(db.ContactUss.Where(contactUs => contactUs.id == key));
        }

        // PUT: odata/ContactUs(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<ContactUs> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ContactUs contactUs = await db.ContactUss.FindAsync(key);
            if (contactUs == null)
            {
                return NotFound();
            }

            patch.Put(contactUs);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactUsExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(contactUs);
        }

        // POST: odata/ContactUs
        public async Task<IHttpActionResult> Post(ContactUs contactUs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            contactUs.CreationDate = DateTime.Now;
            contactUs.LastModificationDate = DateTime.Now;
            contactUs.Creator = core.getCurrentUser().Id;
            contactUs.Modifier = core.getCurrentUser().Id;
            
            db.ContactUss.Add(contactUs);
            await db.SaveChangesAsync();

            return Created(contactUs);
        }

        // PATCH: odata/ContactUs(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ContactUs> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ContactUs contactUs = await db.ContactUss.FindAsync(key);
            if (contactUs == null)
            {
                return NotFound();
            }

            patch.Patch(contactUs);

            try
            {
                contactUs.LastModificationDate = DateTime.Now;
                contactUs.Modifier = core.getCurrentUser().Id;
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactUsExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(contactUs);
        }

        // DELETE: odata/ContactUs(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            ContactUs contactUs = await db.ContactUss.FindAsync(key);
            if (contactUs == null)
            {
                return NotFound();
            }

            db.ContactUss.Remove(contactUs);
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

        private bool ContactUsExists(int key)
        {
            return db.ContactUss.Count(e => e.id == key) > 0;
        }
    }
}
