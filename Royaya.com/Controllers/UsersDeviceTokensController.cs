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
    builder.EntitySet<UsersDeviceTokens>("UsersDeviceTokens");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UsersDeviceTokensController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CoreController core = new CoreController();

        // GET: odata/UsersDeviceTokens
        [EnableQuery]
        public IQueryable<UsersDeviceTokens> GetUsersDeviceTokens()
        {
            return db.UsersDeviceTokens;
        }

        // GET: odata/UsersDeviceTokens(5)
        [EnableQuery]
        public SingleResult<UsersDeviceTokens> GetUsersDeviceTokens([FromODataUri] int key)
        {
            return SingleResult.Create(db.UsersDeviceTokens.Where(usersDeviceTokens => usersDeviceTokens.id == key));
        }

        // PUT: odata/UsersDeviceTokens(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<UsersDeviceTokens> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UsersDeviceTokens usersDeviceTokens = await db.UsersDeviceTokens.FindAsync(key);
            if (usersDeviceTokens == null)
            {
                return NotFound();
            }

            patch.Put(usersDeviceTokens);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersDeviceTokensExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(usersDeviceTokens);
        }

        // POST: odata/UsersDeviceTokens
        public async Task<IHttpActionResult> Post(UsersDeviceTokens usersDeviceTokens)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (usersDeviceTokens.token == null)
            {
                return BadRequest("Token can't be null!");
            }
            if (usersDeviceTokens.UserId == null)
            {
                return BadRequest("User can't be null!");
            }
            ApplicationUser user = db.Users.Find(usersDeviceTokens.UserId);
            if (user == null)
            {
                return BadRequest("No matching user!");
            }
            UsersDeviceTokens temp = db.UsersDeviceTokens.Where(a => a.UserId.Equals(usersDeviceTokens.UserId)).FirstOrDefault();
            if (temp != null)
            {
                temp.token = temp.token + ";" + usersDeviceTokens.token;
                temp.LastModificationDate = DateTime.Now;
                temp.Modifier = core.getCurrentUser().Id;
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(temp);
            }
            else
            {
                usersDeviceTokens.CreationDate = DateTime.Now;
                usersDeviceTokens.LastModificationDate = DateTime.Now;
                usersDeviceTokens.Creator = core.getCurrentUser().Id;
                usersDeviceTokens.Modifier = core.getCurrentUser().Id;

                db.UsersDeviceTokens.Add(usersDeviceTokens);
                await db.SaveChangesAsync();
                return Created(usersDeviceTokens);
            }

            
        }

        // PATCH: odata/UsersDeviceTokens(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<UsersDeviceTokens> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UsersDeviceTokens usersDeviceTokens = await db.UsersDeviceTokens.FindAsync(key);
            if (usersDeviceTokens == null)
            {
                return NotFound();
            }

            patch.Patch(usersDeviceTokens);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersDeviceTokensExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(usersDeviceTokens);
        }

        // DELETE: odata/UsersDeviceTokens(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            UsersDeviceTokens usersDeviceTokens = await db.UsersDeviceTokens.FindAsync(key);
            if (usersDeviceTokens == null)
            {
                return NotFound();
            }

            db.UsersDeviceTokens.Remove(usersDeviceTokens);
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

        private bool UsersDeviceTokensExists(int key)
        {
            return db.UsersDeviceTokens.Count(e => e.id == key) > 0;
        }
    }
}
